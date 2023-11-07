using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TodoEf;

public class User
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; } = "";

    List<Todo> Todos { get; set; } = new();
}

public class Tag
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string Description { get; set; } = "";

    List<Todo> Todos { get; set; } = new();
}

public class Todo
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string Title { get; set; } = "";

    public bool IsCompleted { get; set; }

    public int UserId { get; set; }

    public User? User { get; set; }

    public List<Tag> Tags { get; set; } = new();
}

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Todo> Todos => Set<Todo>();
    public DbSet<Tag> Tags => Set<Tag>();
}
