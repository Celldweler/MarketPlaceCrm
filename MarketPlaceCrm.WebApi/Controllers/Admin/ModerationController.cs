using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MarketPlaceCrm.Data.Context;
using MarketPlaceCrm.Data.Entities;
using MarketPlaceCrm.Data.Entities.Moderation;
using MarketPlaceCrm.Data.Enums;
using MarketPlaceCrm.WebApi.SignalrHubs;
using MarketPlaceCrm.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MarketPlaceCrm.WebApi.Controllers
{
    [Route("api/moderationItems")]
    public class ModerationController : ApiBaseController
    {
        private readonly AppDbContext _ctx;

        public ModerationController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IActionResult All()
        {
            var moderationItems = _ctx.ModerationItems
                .Include(x => x.Reviews)
                .Include(x => x.User)
                .Select(x => new
                {
                    x.Id,
                    Created = x.Created.ToString("dd.mm.yyyy hh:mm"),
                    x.Type,
                    x.CurrentId,
                    x.TargetId,
                    CreatedBy = x.UserId,
                    x.User.Email,
                    x.IsRejected,
                    Reviews = x.Reviews.Select(y => new
                    {
                        y.Id,
                        y.UserId,
                        CreatedAt = y.Updated.ToString("dd.mm.yyyy hh:mm"),
                        y.ModerationItemID,
                        y.Comment,
                        y.ReviewStatus,
                    })
                })
                .AsEnumerable();

            return Ok(moderationItems);
        }

        public class ReviewForm
        {
            public ReviewStatus ReviewStatus { get; set; }
            public int UserId { get; set; }
            public string Comment { get; set; }
            public int ModerationItemId { get; set; }
        }

    
        // 0 -approved, 1 - reject
        [HttpPut]
        public async Task<IActionResult> Moderate([FromForm] ReviewForm reviewForm,
            [FromServices] IHubContext<CustomHub> hubContext)
        {
            var modItem = _ctx.ModerationItems.FirstOrDefault(x => x.Id == reviewForm.ModerationItemId);
            
            modItem.Reviews.Add(new Review
            {
                Comment = reviewForm.Comment,
                ReviewStatus = reviewForm.ReviewStatus,
                UserId = reviewForm.UserId
            });
            
            if (modItem.Type == ModerationTypes.NewAddedProduct && reviewForm.ReviewStatus == ReviewStatus.Approved)
            {
                var product = _ctx.Products.FirstOrDefault(x => x.Id == modItem.CurrentId);
                product.IsPublished = true;
            }

            var user = _ctx.ApplicationUsers.FirstOrDefault(x => x.Id == reviewForm.UserId); 
            _ctx.Notifications.Add(new Notification
            {
                SenderId = reviewForm.UserId,
                ReceiverId = modItem.UserId,
                JsonData = $"{user.Email} reply to your action {modItem.Type} '{reviewForm.Comment}'",
                IsRead = false,
            });
            await _ctx.SaveChangesAsync();

            return Ok();
        }
    }
}