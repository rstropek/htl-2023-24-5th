using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TodoEf;

public class User
{
    // Note that we use the property with name "Id" here. This will
    // become the primary key column in the database table. It will
    // receive auto-incremeted values.
    public int Id { get; set; }

    // Note that you can use data annotations to specify the additional
    // properties like max length, required, etc.
    [MaxLength(50)]
    public string Name { get; set; } = "";

    // There is a 1:n relationship between User and Todo. Therefore,
    // the User class has a property of type List<Todo> to hold the
    // Todo instances that belong to the user.
    public List<Todo> Todos { get; set; } = new();
}

public class Tag
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string Description { get; set; } = "";

    // There is a n:m relationship between Todo and Tag. Therefore,
    // the Tag class has a property of type List<Todo> to hold the
    // Todo instances that are tagged with the tag.
    public List<Todo> Todos { get; set; } = new();
}

public class Todo
{
    public int Id { get; set; }

    public string Title { get; set; } = "";

    public bool IsCompleted { get; set; }

    // There is a 1:n relationship between User and Todo. Therefore,
    // the Todo class has a property of type User to hold the
    // User instance that owns the todo. Additionally, UserId is
    // a foreign key column in the database table.
    public int UserId { get; set; }
    public User? User { get; set; }

    // There is a n:m relationship between Todo and Tag. Therefore,
    // the Todo class has a property of type List<Tag> to hold the
    // Tag instances that are assigned to the todo.
    public List<Tag> Tags { get; set; } = new();
}

public class ApplicationDbContext : DbContext
{
    // Note that constructors in Entity Framework always look as follows:
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // The DbSet properties are used to access the database tables.
    public DbSet<User> Users => Set<User>();
    public DbSet<Todo> Todos => Set<Todo>();
    public DbSet<Tag> Tags => Set<Tag>();

    public async Task CleanDatabase()
    {
        // Note that this is how you can efficiently delete all rows from a table:
        await Todos.ExecuteDeleteAsync();
        await Tags.ExecuteDeleteAsync();
        await Users.ExecuteDeleteAsync();
    }
}
