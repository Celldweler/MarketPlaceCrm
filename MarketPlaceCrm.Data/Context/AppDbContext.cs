using MarketPlaceCrm.Data.Entities;
using MarketPlaceCrm.Data.Entities.Moderation;
using MarketPlaceCrm.Data.EntityConfigurations;
using MarketPlaceCrm.Data.SeedData;
using MarketPlaceCrm.Data.Utils;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceCrm.Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategoryRelationship> ProductCategoryRelationships { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderHistoryDetail> OrderHistory { get; set; }
        public DbSet<ModerationItem> ModerationItems { get; set; }
        public DbSet<BackInStockSubscription> BackInStockSubscriptions { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(Util.loggerFactory);
            optionsBuilder.EnableSensitiveDataLogging();
            // optionsBuilder.UseSqlServer(Settings.ConnectionString);
            
            // for dev and testing purpose uncommented 
            optionsBuilder.UseInMemoryDatabase("Dev");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new StockConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryRelationshipConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            // modelBuilder.ApplyConfiguration(new ShipperConfiguration());
            
            modelBuilder.Seed();
        }
    }
}