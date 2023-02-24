using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MarketPlaceCrm.Data.Entities
{
    public  class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(x => x.Id);
            
        }
    }

    public class Notification : IEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ApplicationUser Sender { get; set; }
        public int SenderId { get; set; }
        
        public ApplicationUser Receiver { get; set; }
        public int ReceiverId { get; set; }

        public NotificationType Type { get; set; }
        public string JsonData { get; set; }

        public bool IsRead { get; set; } 
    }

    public enum NotificationType
    {
        UserCommentPassedModeration,
        InvitationToModeration,
        ChangeOrderStatus,
        ProductBackInStock
    }
}