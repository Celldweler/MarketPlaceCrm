using System.Collections.Generic;
using MarketPlaceCrm.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlaceCrm.Data.Entities
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Notifications)
                .WithOne(y => y.Receiver);
            
            
        }
    }
    public class ApplicationUser : BaseEntity<int>
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string ProfileImageUrl { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string ZipCode { get; set; }
        
        public string Role { get; set; }

        public List<Notification> Notifications { get; set; } = new List<Notification>();

        // it is only news which user unread yet
        // public List<Notification> ReceivedNotifications { get; set; }
    }

    public class Customer : ApplicationUser
    {
        public List<Order> Orders { get; set; }
        public List<OrderHistoryDetail> OrderHistory { get; set; }
        public List<Comment> MyComments { get; set; }
        // public List<object> WishList { get; set; }
    }
    public class StoreOwner : ApplicationUser
    {
        public List<object> Staffs { get; set; }
        public List<Store> Stores { get; set; }
    }
}