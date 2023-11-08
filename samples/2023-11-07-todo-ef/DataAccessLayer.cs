using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace TodoEf;

public record TodoFilter(string? TitleFilter, bool? DoneFilter, int? Skip, int? Take);

public interface IDataAccess
{
    IQueryable<User> GetUserById(int id);
    
    IQueryable<Todo> GetFilteredTodos(TodoFilter filter);

    IQueryable<Todo> GetTodoById(int id);

    IQueryable<Tag> GetTagsByIds(IEnumerable<int> ids);

    Task CleanDatabase();

    DbSet<Todo> Todos { get; }

    Task SaveChangesAsync();

    DatabaseFacade Database { get; }
}

// Note that in larger, more complex applications, you would probably
// split data access into multiple classes. Sometimes, you would even
// split it into multiple projects.
public class DataAccess : IDataAccess
{
    private readonly ApplicationDbContext dbContext;

    public DataAccess(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IQueryable<User> GetUserById(int id)
    {
        return dbContext.Users.Where(u => u.Id == id);
    }

    public IQueryable<Todo> GetFilteredTodos(TodoFilter filter)
    {
        // Note that you can build a query dynamically using the following pattern:
        IQueryable<Todo> query = dbContext.Todos;

        if (!string.IsNullOrWhiteSpace(filter.TitleFilter))
        {
            query = query.Where(t => t.Title.Contains(filter.TitleFilter));
        }

        if (filter.DoneFilter.HasValue)
        {
            query = query.Where(t => t.IsCompleted == filter.DoneFilter.Value);
        }

        if (filter.Skip.HasValue)
        {
            query = query.Skip(filter.Skip.Value);
        }

        if (filter.Take.HasValue)
        {
            query = query.Take(filter.Take.Value);
        }

        return query;
    }

    public IQueryable<Todo> GetTodoById(int id)
    {
        return dbContext.Todos.Where(u => u.Id == id);
    }

    public IQueryable<Tag> GetTagsByIds(IEnumerable<int> ids)
    {
        return dbContext.Tags.Where(t => ids.Contains(t.Id));
    }

    public async Task CleanDatabase()
    {
        // Note that this is how you can efficiently delete all rows from a table:
        await dbContext.Todos.ExecuteDeleteAsync();
        await dbContext.Tags.ExecuteDeleteAsync();
        await dbContext.Users.ExecuteDeleteAsync();
    }

    public DbSet<Todo> Todos => dbContext.Todos;

    public async Task SaveChangesAsync() => await dbContext.SaveChangesAsync();

    public DatabaseFacade Database => dbContext.Database;
}

public static class SelectExtensions
{
    private static readonly Expression<Func<Todo, TodoSummary>> toSummary = t => new TodoSummary(
            t.Id,
            t.Title,
            t.IsCompleted,
            t.UserId,
            t.User!.Name,
            t.Tags.Select(t => t.Description));
    private static readonly Func<Todo, TodoSummary> toSummaryCompiled = toSummary.Compile();

    // Note that in this case, we use an EXTENSION METHOD to add a new
    // method to the IQueryable<Todo> interface.
    public static IQueryable<TodoSummary> SelectSummary(this IQueryable<Todo> query) =>
        query.Select(toSummary);

    public static TodoSummary ToSummary(this Todo t) => toSummaryCompiled(t);
}

public record TodoSummary(int Id, string Title, bool IsCompleted, int UserId, string User, IEnumerable<string> Tags);
