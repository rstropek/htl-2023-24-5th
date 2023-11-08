using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoEf;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IDataAccess, DataAccess>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Note that it is ALLOWED to return an EF DbSet or the result of a
// LINQ query from a handler method. In this case, the handler function
// does NOT need to be async.

// Note that in most non-trivial applications it is NOT RECOMMENDED to
// return the EF DbSet or the result of a LINQ query from a handler method.
// Instead, you should return a DTO (data transfer object) that contains
// only the data that is needed by the client. However, IF you app IS
// SIMPLE enough, you can return DB results directly from the handler.
app.MapGet("/users", (ApplicationDbContext dbContext) => dbContext.Users);

app.MapGet("/users/{id}", async (IDataAccess da, int id) =>
{
    // Note that you SHOULD use .AsNoTracking() if you read data from the DB
    // in a handler method and you DON'T plan to update the data. This will
    // improve the performance of the application.

    // Note that you MUST ALWAYS use async/await when interacting with the DB.
    // NEVER use the blocking methods (e.g. FirstOrDefault()) without async/await!
    var user = await da.GetUserById(id).AsNoTracking().FirstOrDefaultAsync();
    if (user is null)
    {
        return Results.NotFound();
    }

    // Here we return the user object directly again. Remember what we said
    // above: In most non-trivial applications, you should return a DTO instead.
    return Results.Ok(user);
});

app.MapGet("todos", (IDataAccess da,
    [FromQuery(Name = "q")] string? titleFilter,
    [FromQuery(Name = "done")] bool? doneFilter,
    [FromQuery()] int? skip,
    [FromQuery()] int? take) =>
{
    var query = da.GetFilteredTodos(new(titleFilter, doneFilter, skip, take)).AsNoTracking();

    // As mentioned before, it is allowed to return a query from a handler method.
    // Note that you MUST NOT call ToArray() or ToList() on the query. If you need
    // to get the result of the query in your handler, you MUST use the async/await
    // (i.e. ToArrayAsync() or ToListAsync()).
    return query.SelectSummary();
});

app.MapGet("/todos/{id}", async (IDataAccess da, int id) =>
{
    // Note that you can load related entities using the Include() method.
    // This will result in a JOIN in the query.
    var todo = await da.GetTodoById(id)
        .AsNoTracking()
        .SelectSummary()
        .FirstOrDefaultAsync();
    if (todo is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(todo);
});

app.MapPost("todos", async (IDataAccess da, AddTodoDto newTodo) =>
{
    // Ensure that given user ID exists. Note that we MUST NOT USE
    // AsNoTracking() here because the user is involved in an insert
    // operation later (as a referenced object).
    var user = await da.GetUserById(newTodo.UserId).FirstOrDefaultAsync();
    if (user is null)
    {
        return Results.BadRequest($"Invalid user ID {newTodo.UserId}");
    }

    // Read the referenced tags from the DB
    var tags = await da.GetTagsByIds(newTodo.TagIds).ToListAsync();
    var missingTags = newTodo.TagIds.Except(tags.Select(t => t.Id)).ToArray();
    if (missingTags.Length > 0)
    {
        return Results.BadRequest($"Invalid tag IDs: {string.Join(", ", missingTags)}");
    }

    var todo = new Todo
    {
        Title = newTodo.Title,
        IsCompleted = false,
        User = user,
        Tags = tags
    };

    await da.Todos.AddAsync(todo);
    await da.SaveChangesAsync();

    return Results.Created($"/todos/{todo.Id}", todo.ToSummary());
});

app.MapPatch("/todos/{id}", async (IDataAccess da, int id, TodoPatchDto patch) =>
{
    // Note how we use the .Include() method to load the related entities.
    var todo = await da.GetTodoById(id)
        .Include(t => t.User)
        .Include(t => t.Tags)
        .FirstOrDefaultAsync();
    if (todo is null)
    {
        return Results.NotFound();
    }

    if (patch.Title is not null) { todo.Title = patch.Title; }
    if (patch.IsCompleted is not null) { todo.IsCompleted = patch.IsCompleted.Value; }

    await da.SaveChangesAsync();

    return Results.Ok(todo.ToSummary());
});

app.MapDelete("/todos/{id}", async (IDataAccess da, int id) =>
{
    var todo = await da.GetTodoById(id).FirstOrDefaultAsync();
    if (todo is null)
    {
        return Results.NotFound();
    }

    da.Todos.Remove(todo);
    await da.SaveChangesAsync();

    return Results.NoContent();
});

app.MapPost("fill", async (IDataAccess da) =>
{
    // Note that this is how you can enclose multiple DB operations in a transaction:
    using var transaction = await da.Database.BeginTransactionAsync();

    try
    {
        await da.CleanDatabase();

        // Note that we use the Bogus library to generate fake data.
        // Very useful for testing purposes!
        var userFaker = new Faker<User>().RuleFor(u => u.Name, f => f.Person.FullName);
        var users = userFaker.Generate(10);

        // Note that in this case, there is NO NEED to insert the users manually.
        // Todo items will reference the users and if the users are not yet in the DB, 
        // they will be inserted automatically.

        var tagFaker = new Faker<Tag>().RuleFor(t => t.Description, f => f.Lorem.Word());
        var tags = tagFaker.Generate(10);

        // Same with tags. No need to insert them manually. They will be inserted
        // automatically if they are referenced by a todo item.

        var todos = new Faker<Todo>()
            .RuleFor(t => t.Title, f => f.Lorem.Sentence())
            .RuleFor(t => t.IsCompleted, f => f.Random.Bool())
            // Note that we can easily link the new Todo instance to a User and
            // multiple Tags. All you need is to assign the corresponding properties.
            .RuleFor(t => t.User, f => f.PickRandom(users))
            .RuleFor(t => t.Tags, f => f.PickRandom(tags, f.Random.Number(1, 3)).ToList())
            .Generate(100);

        // Note that this is how you can efficiently insert multiple rows into a DB table:
        await da.Todos.AddRangeAsync(todos);

        // Note that you MUST ALWAYS call SaveChangesAsync() after adding, updating,
        // or deleting rows in the DB. Otherwise, the changes will NOT be persisted.
        await da.SaveChangesAsync();

        // If we reach this point, all DB operations were successful and we can
        // commit the transaction.
        await da.Database.CommitTransactionAsync();
    }
    catch
    {
        // If an exception occurs, we need to roll back the transaction.
        await da.Database.RollbackTransactionAsync();
        throw;
    }
});

await app.RunAsync();

public record AddTodoDto(string Title, int UserId, int[] TagIds);

public record TodoPatchDto(string? Title, bool? IsCompleted);
