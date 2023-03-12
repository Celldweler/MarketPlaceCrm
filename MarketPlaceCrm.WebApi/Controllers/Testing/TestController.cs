using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketPlaceCrm.Data.Context;
using MarketPlaceCrm.Data.Entities;
using MarketPlaceCrm.WebApi.Services;
using MarketPlaceCrm.WebApi.SignalrHubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MarketPlaceCrm.WebApi.Controllers.Testing
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly NotificationsService _notificationsService;
        private readonly AppDbContext _ctx;

        public TestController(NotificationsService notificationsService, AppDbContext ctx)
        {
            _notificationsService = notificationsService;
            _ctx = ctx;
        }
        
        [HttpGet]
        [Route("/api/test/[action]")]

        public async Task<IActionResult> Send()
        {
            var str = "";
            if (HttpContext.Request.Query.ContainsKey("msg"))
            {
                str = HttpContext.Request.Query["msg"];
            }
            
            await _notificationsService.NotifyClient(str);
            
            return Ok($"send actiona called your msg from query: {str}");
        }
        
        [HttpGet("shippingMethods")]
        public IEnumerable<ShippingMethod> GetShippingMethods() => _ctx.ShippingMethods.AsEnumerable();
        
        [HttpGet("")]
        [Authorize]
        public IActionResult Get() => Ok($"{nameof(TestController).ToLower()} is work!");
    }
}