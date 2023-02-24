using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Sandbox
{
    public class Order
    {
        public int OrderId { get; set; }
        public string Item { get; set; }
        public int Quantity { get; set; }

        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
    }

    public class Customer
    {
        public Customer()
        {
        }

        public Customer(int userId) => this.Id = userId;

        public int Id { get; set; }
        public string UserName { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ProductCategory> Categories { get; set; }
    }
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductCategory> Products { get; set; }
    }
    public class ProductCategory
    {
        public Product Product { get; set; }
        public Category Category { get; set; }
        
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
    }
    public class AppDbContext : DbContext
    {
        // public DbSet<Order> Orders { get; set; }
        // public DbSet<Customer> Customers { get; set; }
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            // optionsBuilder.UseInMemoryDatabase("dev");
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\MSSQLLocalDB;Database=SandboxTestDB;Trusted_Connection=True;");

            // for dev and testing purpose uncommented 
            // optionsBuilder.UseInMemoryDatabase("Dev");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // builder.Entity<Customer>()
            //     .HasMany(x => x.Orders)
            //     .WithOne(y => y.Customer)
            //     .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Product>().HasKey(x => x.Id);
            builder.Entity<Product>()
                .Property(x => x.Id)
                .ValueGeneratedNever();
            
            builder.Entity<Category>().HasKey(x => x.Id);
            builder.Entity<Category>()
                .Property(x => x.Id)
                .ValueGeneratedNever();
            
            builder.Entity<ProductCategory>()
                .HasKey(x => new { x.CategoryId, x.ProductId });
       
            builder.Entity<ProductCategory>()
                .HasOne(x => x.Product)
                .WithMany(x => x.Categories)
                .HasForeignKey(x => x.ProductId);
            
            builder.Entity<ProductCategory>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId);
            //
            // builder.Entity<Order>()
            //     .HasOne(x => x.Customer)
            //     .WithMany(y => y.Orders)
            //     .OnDelete(DeleteBehavior.Cascade);

            // builder.Entity<Customer>(x =>
            // {
            //     x.HasData(
            //         new Customer
            //         {
            //             Id = 1, UserName = "test",
            //             Orders = new List<Order>()
            //             {
            //                 new Order { OrderId = 1001, Item = "t-shirt", Quantity = 1, CustomerId = 1 },
            //                 new Order { OrderId = 1002, Item = "hoodie", Quantity = 3, CustomerId = 1 },
            //                 new Order { OrderId = 1003, Item = "pant", Quantity = 2, CustomerId = 1 }
            //             }
            //         }
            //     );
            // });
        }
    }
}