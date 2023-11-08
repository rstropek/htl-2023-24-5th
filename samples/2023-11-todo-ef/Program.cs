using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoEf;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
app.MapGet("/users", (ApplicationDbContext dc) => dc.Users);

app.MapGet("/users/{id}", async (ApplicationDbContext dc, int id) =>
{
    // Note that you SHOULD use .AsNoTracking() if you read data from the DB
    // in a handler method and you DON'T plan to update the data. This will
    // improve the performance of the application.

    // Note that you MUST ALWAYS use async/await when interacting with the DB.
    // NEVER use the blocking methods (e.g. FirstOrDefault()) without async/await!
    var user = await dc.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
    if (user is null)
    {
        return Results.NotFound();
    }

    // Here we return the user object directly again. Remember what we said
    // above: In most non-trivial applications, you should return a DTO instead.
    return Results.Ok(user);
});

app.MapGet("todos", (ApplicationDbContext dc,
    [FromQuery(Name = "q")] string? titleFilter,
    [FromQuery(Name = "done")] bool? doneFilter,
    [FromQuery()] int? skip,
    [FromQuery()] int? take) =>
{
    var query = dc.Todos.AsNoTracking()
        .GetFilteredTodos(new(titleFilter, doneFilter, skip, take));

    // As mentioned before, it is allowed to return a query from a handler method.
    // Note that you MUST NOT call ToArray() or ToList() on the query. If you need
    // to get the result of the query in your handler, you MUST use the async/await
    // (i.e. ToArrayAsync() or ToListAsync()).
    return query.SelectSummary();
});

app.MapGet("/todos/{id}", async (ApplicationDbContext dc, int id) =>
{
    // Note that you can load related entities using the Include() method.
    // This will result in a JOIN in the query.
    var todo = await dc.Todos
        .AsNoTracking()
        .SelectSummary()
        .Where(t => t.Id == id)
        .FirstOrDefaultAsync();
    if (todo is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(todo);
});

app.MapPost("todos", async (ApplicationDbContext dc, AddTodoDto newTodo) =>
{
    // Ensure that given user ID exists. Note that we MUST NOT USE
    // AsNoTracking() here because the user is involved in an insert
    // operation later (as a referenced object).
    var user = await dc.Users.FirstOrDefaultAsync(u => u.Id == newTodo.UserId);
    if (user is null)
    {
        return Results.BadRequest($"Invalid user ID {newTodo.UserId}");
    }

    // Read the referenced tags from the DB.
    var tags = await dc.Tags
        .Where(t => newTodo.Tags.Contains(t.Description))
        .Distinct()
        .ToListAsync();

    // Create new tags for the tags that don't exist in the DB yet.
    // Note that we do NOT need to insert the new tags manually in the DB.
    // They will be referenced by the new todo item and therefore inserted
    // automatically.
    // Note that the following LINQ statement does NOT access the DB.
    // It is executed in-memory.
    var newTags = newTodo.Tags
        .Except(tags.Select(t => t.Description))
        .Select(t => new Tag { Description = t });
    tags.AddRange(newTags);

    // Construct the new todo item.
    var todo = new Todo
    {
        Title = newTodo.Title,
        IsCompleted = false,
        User = user,
        Tags = tags
    };

    // Add the todo item and save it to the DB.
    await dc.Todos.AddAsync(todo);

    // You MUST, MUST, MUST call SaveChangesAsync() after adding, updating,
    // or deleting rows in the DB. Otherwise, the changes will NOT be persisted.
    await dc.SaveChangesAsync();

    return Results.Created($"/todos/{todo.Id}", todo.ToSummary());
});

// Note that for patching, we use a record in which all updatable properties
// are nullable. This way, we can easily check which properties were updated
// by the client.
app.MapPatch("/todos/{id}", async (ApplicationDbContext dc, int id, TodoPatchDto patch) =>
{
    // Again, we do NOT use AsNoTracking() here because we want to update
    // the todo item later.
    var todo = await dc.Todos.FirstOrDefaultAsync(t => t.Id == id);
    if (todo is null)
    {
        return Results.NotFound();
    }

    if (patch.Title is not null) { todo.Title = patch.Title; }
    if (patch.IsCompleted is not null) { todo.IsCompleted = patch.IsCompleted.Value; }

    await dc.SaveChangesAsync();

    // Note how we can load referenced records from the DB.
    await dc.Entry(todo).Reference(t => t.User).LoadAsync();
    await dc.Entry(todo).Collection(t => t.Tags).LoadAsync();

    return Results.Ok(todo.ToSummary());
});

app.MapDelete("/todos/{id}", async (ApplicationDbContext dc, int id) =>
{
    var todo = await dc.Todos.FirstOrDefaultAsync(t => t.Id == id);
    if (todo is null)
    {
        return Results.NotFound();
    }

    dc.Todos.Remove(todo);
    await dc.SaveChangesAsync();

    return Results.NoContent();
});

app.MapPost("fill", async (ApplicationDbContext dc) =>
{
    // Note that this is how you can enclose multiple DB operations in a transaction:
    using var transaction = await dc.Database.BeginTransactionAsync();

    try
    {
        // Note that this is how you can efficiently delete all rows from a table:
        await dc.Todos.ExecuteDeleteAsync();
        await dc.Tags.ExecuteDeleteAsync();
        await dc.Users.ExecuteDeleteAsync();

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
        await dc.Todos.AddRangeAsync(todos);

        // Note that you MUST ALWAYS call SaveChangesAsync() after adding, updating,
        // or deleting rows in the DB. Otherwise, the changes will NOT be persisted.
        await dc.SaveChangesAsync();

        // If we reach this point, all DB operations were successful and we can
        // commit the transaction.
        await dc.Database.CommitTransactionAsync();
    }
    catch
    {
        // If an exception occurs, we need to roll back the transaction.
        await dc.Database.RollbackTransactionAsync();
        throw;
    }
});

app.MapGet("tags/{tag}/todos", (ApplicationDbContext dc, string? tag) =>
{
    // Note that slightly more advanced query. It starts by filtering the tags.
    // Then, it uses SelectMany() to get all todos that are linked to the filtered tag.
    return dc.Tags
        .AsNoTracking()
        .Where(t => t.Description == tag)
        .SelectMany(t => t.Todos)
        .SelectSummary();
});

app.MapGet("tags/statistics", (ApplicationDbContext dc) =>
{
    // This slightly more advanced query shows how to aggregate values (counting,
    // summing, etc.). In this case, we calculate the number of todos in a subquery.
    return dc.Tags
        .AsNoTracking()
        .Select(t => new
        {
            Tag = t.Description,
            TotalTodos = t.Todos.Count,
            OpenTodos = t.Todos.Count(t => !t.IsCompleted),
            ClosedTodos = t.Todos.Count(t => t.IsCompleted)
        });
});
await app.RunAsync();

public record AddTodoDto(string Title, int UserId, string[] Tags);

public record TodoPatchDto(string? Title, bool? IsCompleted);
