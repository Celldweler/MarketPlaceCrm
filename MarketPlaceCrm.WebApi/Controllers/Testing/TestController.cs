using System.Threading.Tasks;
using MarketPlaceCrm.WebApi.Services;
using MarketPlaceCrm.WebApi.SignalrHubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MarketPlaceCrm.WebApi.Controllers.Testing
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly NotificationsService _notificationsService;

        public TestController(NotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
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
        
        [HttpGet("")]
        public IActionResult Get() => Ok($"{nameof(TestController).ToLower()} is work!");
    }
}