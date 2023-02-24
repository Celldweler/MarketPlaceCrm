using System;
using System.Linq;
using System.Linq.Expressions;
using MarketPlaceCrm.Data.Entities;
using MarketPlaceCrm.WebApi.Services;

namespace MarketPlaceCrm.WebApi.ViewModels.UsersDtos
{
    public static class AppUserDto
    {
        public static Expression<Func<ApplicationUser, object>> BriefInfo = x =>
            new
            {
                x.Id,
                x.Email,
                x.UserName,
                x.Role,
                FullName = $"{x.Name} {x.LastName}",
                x.PhoneNumber,
                x.ProfileImageUrl,
                x.Deleted,
                CreatedAt = x.Created.Parse(),
            };
        
        public static readonly Func<ApplicationUser, object> WithNotifications = x =>
            new
            {
                x.Id,
                x.Email,
                x.UserName,
                x.Role,
                FullName = $"{x.Name} {x.LastName}",
                x.PhoneNumber,
                x.ProfileImageUrl,
                x.Deleted,
                CreatedAt = x.Created.Parse(),
                Notifications = x.Notifications.AsEnumerable().Select(n => new
                {
                    n.Id,
                    n.ReceiverId,
                    n.SenderId,
                    n.Sender.Email,
                    Type = n.Type switch
                    {
                        NotificationType.ChangeOrderStatus => nameof(NotificationType.ChangeOrderStatus),
                        NotificationType.UserCommentPassedModeration => nameof(NotificationType.UserCommentPassedModeration),
                        NotificationType.InvitationToModeration => nameof(NotificationType.InvitationToModeration),
                    },
                    CreatedAt = n.CreatedAt.Parse(),
                    n.IsRead,
                    n.JsonData,
                })
            };
    }
}