using Microsoft.EntityFrameworkCore;
using Paessler.Task.Model.Models;

namespace Paessler.Task.Model;

public class PostgresContext : DbContext
{
    private readonly string _connectionString;
    public DbSet<Order> Orders { get; set; }

    public PostgresContext()
    {
        _connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasMany(o => o.ProductOrdered)
            .WithOne(po => po.Order)
            .HasForeignKey(po => po.order_id);

        modelBuilder.Entity<ProductOrdered>()
            .HasOne(po => po.Product)
            .WithMany()
            .HasForeignKey(po => po.product_id);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.customer_id);
    }
}