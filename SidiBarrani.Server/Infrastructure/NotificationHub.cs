using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SidiBarrani.Server.Infrastructure
{
    public class NotificationHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        //public async Task AddToGameGroupAsync(string group)
        //{
        //    await Groups.AddToGameGroupAsync(Context.ConnectionId, group);
        //}

        //public async Task RemoveFromGameGroupAsync(string group)
        //{
        //    await Groups.RemoveFromGameGroupAsync(Context.ConnectionId, group);
        //}
    }
}
