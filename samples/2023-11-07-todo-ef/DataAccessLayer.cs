using System.Linq.Expressions;

namespace TodoEf;

public record TodoFilter(string? TitleFilter, bool? DoneFilter, int? Skip, int? Take);

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

    public static IQueryable<Todo> GetFilteredTodos(this IQueryable<Todo> todos, TodoFilter filter)
    {
        // Note that you can build a query dynamically using the following pattern:
        IQueryable<Todo> query = todos;

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
}

public record TodoSummary(int Id, string Title, bool IsCompleted, int UserId, string User, IEnumerable<string> Tags);
