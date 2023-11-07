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
app.MapGet("/users", (ApplicationDbContext dbContext) => dbContext.Users);

app.MapGet("/users/{id}", async (ApplicationDbContext dbContext, int id) =>
{
    // Note that you SHOULD use .AsNoTracking() if you read data from the DB
    // in a handler method and you DON'T plan to update the data. This will
    // improve the performance of the application.

    // Note that you MUST ALWAYS use async/await when interacting with the DB.
    // NEVER use the blocking methods (e.g. FirstOrDefault()) without async/await!
    var user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
    if (user is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(user);
});

app.MapGet("todos", (ApplicationDbContext dbContext,
    [FromQuery(Name = "q")] string? titleFilter,
    [FromQuery(Name = "done")] bool? doneFilter) =>
{
    // Note that you can build a query dynamically using the following pattern:
    IQueryable<Todo> query = dbContext.Todos.AsNoTracking();

    if (!string.IsNullOrWhiteSpace(titleFilter))
    {
        query = query.Where(t => t.Title.Contains(titleFilter));
    }

    if (doneFilter.HasValue)
    {
        query = query.Where(t => t.IsCompleted == doneFilter.Value);
    }

    // As mentioned before, it is allowed to return a query from a handler method.
    // Note that you MUST NOT call ToArray() or ToList() on the query. If you need
    // to get the result of the query in your handler, you MUST use the async/await
    // (i.e. ToArrayAsync() or ToListAsync()).
    return query.Select(t => new
    {
        t.Id,
        t.Title,
        t.IsCompleted,
        t.User!.Name // Note that this is how you can retrieve columns from joined tables
    });
});

app.MapPost("fill", async (ApplicationDbContext dbContext) =>
{
    // Note that this is how you can enclose multiple DB operations in a transaction:
    using var transaction = await dbContext.Database.BeginTransactionAsync();

    try
    {
        await dbContext.CleanDatabase();

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
        await dbContext.Todos.AddRangeAsync(todos);

        // Note that you MUST ALWAYS call SaveChangesAsync() after adding, updating,
        // or deleting rows in the DB. Otherwise, the changes will NOT be persisted.
        await dbContext.SaveChangesAsync();

        // If we reach this point, all DB operations were successful and we can
        // commit the transaction.
        await dbContext.Database.CommitTransactionAsync();
    }
    catch
    {
        // If an exception occurs, we need to roll back the transaction.
        await dbContext.Database.RollbackTransactionAsync();
        throw;
    }
});

await app.RunAsync();
