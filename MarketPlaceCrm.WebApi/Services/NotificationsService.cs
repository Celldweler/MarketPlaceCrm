using System.Threading.Tasks;
using MarketPlaceCrm.WebApi.SignalrHubs;
using Microsoft.AspNetCore.SignalR;

namespace MarketPlaceCrm.WebApi.Services
{
    public class NotificationsService
    {
        private readonly IHubContext<CustomHub> _hubContext;

        public NotificationsService(IHubContext<CustomHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyClient(string msg)
        {
            await _hubContext.Clients.All.SendAsync("SendMessage", msg);
        }
    }
}