using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Paessler.Task.Model;
using Paessler.Task.Model.Models;

public class CustomWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development"); // Optional: enables developer exception page

        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<PostgresContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add the in-memory database for tests
            services.AddDbContext<PostgresContext>(options =>
            {
                options.UseInMemoryDatabase("IntegrationTestsDb");
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PostgresContext>();

            db.Database.EnsureCreated();

            db.Customers.AddRange(
                new Customer { id = 1, email = "john.doe@example.com", address = "123 Main St, Berlin", credit_card_number = "CfDJ8Iw3dAU2WBtBuatyvnlSJnGW3WhvkldcB1_HH-IYqIHArl_nSPELbMVIaWgMrAwtpocN8KC7o0XZR7Sykcmjpgfbq4zxQIKvOupsJ6Udd44KWgJ2ZJ85WJpy9H2_kuEe_L1R1IhSrMvd5EIai6PmLxo" }
            );
            db.Products.AddRange(
                new Product { id = 1, name = "Gaming Laptop", inventory_amount = 10, price = 1499.99f },
                new Product { id = 2, name = "Gaming Headphones", inventory_amount = 10, price = 149.99f }
            );
            db.Orders.AddRange(
                new Order { id = 1, customer_id = 1, created_at = DateTime.UtcNow}
            );
            db.OrderedProducts.AddRange(
                new ProductOrdered { id = 1, order_id = 1, product_id = 1, amount = 1, total_price = 1499.99f },
                new ProductOrdered { id = 2, order_id = 1, product_id = 2, amount = 2, total_price = 299.98f }
            );

            db.SaveChanges();
        });
    }
}
