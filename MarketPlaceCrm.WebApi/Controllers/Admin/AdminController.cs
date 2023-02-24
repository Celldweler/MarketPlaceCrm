using System.Linq;
using MarketPlaceCrm.Data.Context;
using MarketPlaceCrm.Data.Entities;
using MarketPlaceCrm.Data.Enums;
using MarketPlaceCrm.WebApi.ViewModels.UsersDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceCrm.WebApi.Controllers
{
    [ApiController]
    // [Authorize(Policy = "admin")]
    [Route("api/admin-panel")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public AdminController(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var users = _ctx.ApplicationUsers
                .Include(x => x.Notifications)
                .ThenInclude(x => x.Sender)
                .AsEnumerable()
                .Select(AppUserDto.WithNotifications);
            
            return Ok(users);
        }

        [HttpGet("allModerators")]
        public IActionResult GetAllModerators()
        {
            var moderators = _ctx.ApplicationUsers
                .Where(x => x.Role == UserRoles.Moderator)
                .AsEnumerable()
                .Select(AppUserDto.BriefInfo.Compile());
            
            return Ok(moderators);
        }

        public class InviteModeratorForm
        {
            public string Email { get; set; }
            public int SenderId { get; set; }
        }
        [HttpPost("moderators")]
        public IActionResult InviteModerator([FromForm]InviteModeratorForm moderatorForm)
        {
            var user = _ctx.ApplicationUsers.FirstOrDefault(x => x.Email.Equals(moderatorForm.Email));
            if (user is null)
                return BadRequest($"user with email {moderatorForm.Email} not exist");

            user.Role = UserRoles.Moderator;

            var adminSender = _ctx.ApplicationUsers.FirstOrDefault(x => x.Id.Equals(moderatorForm.SenderId));
            _ctx.Notifications.Add(new Notification
            {
                SenderId = moderatorForm.SenderId,
                ReceiverId = user.Id,
                Type = NotificationType.InvitationToModeration,
                IsRead = false,
                JsonData = $"{adminSender.Email} made you a moderator"
            });

            _ctx.SaveChanges();

            return Ok();
        }
    }
}