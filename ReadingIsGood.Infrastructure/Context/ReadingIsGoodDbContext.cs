using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReadingIsGood.Domain.Customer;
using ReadingIsGood.Domain.Order;
using ReadingIsGood.Domain.Product;

namespace ReadingIsGood.Infrastructure.Context
{
    public class ReadingIsGoodDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<Customer>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Products
            builder.Entity<Product>().ToTable("Products");

            //Orders
            builder.Entity<Order>().ToTable("Orders");
            builder.Entity<OrderItem>().ToTable("OrderItems").HasIndex(e => e.ProductId).IsUnique(false);

            // Customers
            builder.Entity<Customer>().ToTable("Customers");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("CustomerRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("CustomerClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("CustomerLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("CustomerTokens");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");

            //Relations
            builder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(e => e.Customer);

            builder.Entity<Order>()
                .HasMany(c => c.OrderItems)
                .WithOne(e => e.Order);


            builder.Entity<Product>()
                .HasKey(s => s.ProductId);

            builder.Entity<Product>()
                .HasOne<OrderItem>(p => p.OrderItem)
                .WithOne(s => s.Product);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            var dbHost = Environment.GetEnvironmentVariable("DbHost");
            var dbPort = Environment.GetEnvironmentVariable("DbPort");
            var dbPassword = Environment.GetEnvironmentVariable("DbPassword");

            if (dbHost != null && dbPort != null)
            {
                builder.UseNpgsql($"Host={dbHost};Port={dbPort};Database=ReadingIsGood;Username=postgres;Password={dbPassword}")
                    .UseSnakeCaseNamingConvention();
            }
            else
            {
                builder.UseNpgsql($"Host=db;Port=5432;Database=ReadingIsGood;Username=postgres;Password=postgres")
                    .UseSnakeCaseNamingConvention();
            }
        }
    }
}