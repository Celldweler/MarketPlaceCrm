using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace MarketPlaceCrm.WebApi.SignalrHubs
{
    public interface IClientInterface
    {
        Task ClientHook(Data data);
    }
    
    public class CustomHub : Hub
    {
        private readonly ILogger<CustomHub> _logger;

        public CustomHub(ILogger<CustomHub> logger) => _logger = logger;

        public void ServerHook(Data data)
        {
            _logger.LogInformation("Receiving data: {0}, {1}", data, Context.ConnectionId);
        }

        public async Task SendMessage(string message)
        {
            _logger.LogInformation("sendMessage called on server by client connection with id: " + Context.ConnectionId);
            Console.WriteLine("Message received");
            
            await Clients.All.SendAsync("SendMessage", message);
        }
        
        public async Task NotifyCustomer(string message, string connId)
        {
            await Clients.Client(connId).SendAsync("onNewNotification", message);
        }

        // public Task PingAll()
        // {
        //     _logger.LogInformation("pinging everyone");
        //     return Clients.All.ClientHook(new Data(111, "ping all"));
        // }
        //
        // public Task SelfPing()
        // {
        //     _logger.LogInformation("self pinging");
        //     return Clients.Caller.ClientHook(new Data(222, "self ping"));
        // }

        [HubMethodName("invocation_with_return")]
        public Data JustAFunction()
        {
            return new Data(1, "returned data from JustAFunction");
        }

        // -------------------------------
        // on connected/disconnected hooks
        // -------------------------------
        public override Task OnConnectedAsync()
        {
            System.Console.WriteLine("New Connection Started: " + Context.ConnectionId);
            Clients.Caller.SendAsync("NewConnection", "New Connection Successfull", Context.ConnectionId);
            
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {

            return Task.CompletedTask;
        }
    }
}