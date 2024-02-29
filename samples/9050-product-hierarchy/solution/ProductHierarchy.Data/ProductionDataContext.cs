using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ProductHierarchy.Data;

public class ProductionDataContext(DbContextOptions<ProductionDataContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Rebate> Rebates => Set<Rebate>();
    public DbSet<ProductHierarchy> ProductHierarchies => Set<ProductHierarchy>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ProductNumber).IsRequired();
            entity.Property(e => e.Manufacturer).IsRequired();
        });

        modelBuilder.Entity<Rebate>(entity =>
        {
            entity.ToTable("Rebate");
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ProductID).IsRequired();
            entity.Property(e => e.MinQuantity).IsRequired();
            entity.Property(e => e.RebatePerc).IsRequired();
            entity.HasOne(e => e.Product).WithMany(e => e.Rebates).HasForeignKey(e => e.ProductID);
        });

        modelBuilder.Entity<ProductHierarchy>(entity =>
        {
            entity.ToTable("ProductHierarchy");
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Amount).IsRequired();
            entity.HasOne(e => e.ParentProduct).WithMany(e => e.ChildProducts).HasForeignKey(e => e.ParentProductID);
            entity.HasOne(e => e.ChildProduct).WithMany(e => e.ParentProducts).HasForeignKey(e => e.ChildProductID);
        });
    }
}

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ProductionDataContext>
{
    public ProductionDataContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ProductionDataContext>();
        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

        return new ProductionDataContext(optionsBuilder.Options);
    }
}