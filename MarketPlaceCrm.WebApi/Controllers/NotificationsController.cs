using System;
using System.Linq;
using MarketPlaceCrm.Data.Context;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceCrm.WebApi.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/notifications")]
    public class NotificationsController : ApiBaseController
    {
        private readonly AppDbContext _ctx;

        public NotificationsController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("{userId}")]
        public IActionResult GetNotifications(int userId)
        {
            if (userId <= 0) return BadRequest("invalid userId");

            var notifications = _ctx.Notifications
                .Where(x => x.ReceiverId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .AsEnumerable();
            
            return Ok(notifications);
        }

        [HttpGet("unreadNumber/{userId}")]
        public IActionResult GetUnreadNotificationsNumber(int userId)
        {
            var count = _ctx.Notifications.Count(x => !x.IsRead && x.ReceiverId == userId);

            return Ok(count);
        }
        
       
        [HttpPut("markAsRead/{notificationId}")]
        public IActionResult MarkNotificationAsRead(int notificationId)
        {
            var notification = _ctx.Notifications.FirstOrDefault(x => x.Id == notificationId);
            notification.IsRead = true;
            _ctx.SaveChanges();
    
            return Ok(notification);
        }
    }
}