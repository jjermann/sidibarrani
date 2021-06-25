using System;
using System.Threading.Tasks;
using SidiBarrani.Shared.Model.Connection;

namespace SidiBarrani.Application.Services
{
    public interface IServerConnectionService
    {
        Task AddToGameGroupAsync(string playerConnectionId, Guid gameId);
        Task RemoveFromGameGroupAsync(string playerConnectionId, Guid gameId);
        Task SendToGameGroupAsync(ConnectionEvent connectionEvent, Guid gameId);
        Task SendToAllAsync(ConnectionEvent connectionEvent);
    }
}
