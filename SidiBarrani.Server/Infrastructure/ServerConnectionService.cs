using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SidiBarrani.Application.Services;
using SidiBarrani.Shared.Model.Connection;

namespace SidiBarrani.Server.Infrastructure
{
    public class ServerConnectionService : IServerConnectionService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
       

        public ServerConnectionService(
            IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task AddToGameGroupAsync(string playerConnectionId, Guid gameId)
        {
            await _hubContext.Groups.AddToGroupAsync(playerConnectionId, gameId.ToString());
        }

        public async Task RemoveFromGameGroupAsync(string playerConnectionId, Guid gameId)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(playerConnectionId, gameId.ToString());
        }

        public async Task SendToGameGroupAsync(ConnectionEvent connectionEvent, Guid gameId)
        {
            await _hubContext.Clients.All.SendAsync(connectionEvent.ToString());
            //await _hubContext.Clients.Group(gameId.ToString()).SendAsync(connectionEvent.ToString());
        }

        public async Task SendToAllAsync(ConnectionEvent connectionEvent)
        {
            await _hubContext.Clients.All.SendAsync(connectionEvent.ToString());
        }
    }
}
