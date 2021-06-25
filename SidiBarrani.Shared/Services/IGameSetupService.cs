using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SidiBarrani.Shared.Model.Setup;

namespace SidiBarrani.Shared.Services
{
    public interface IGameSetupService
    {
        Task<GameSetup> OpenGameSetupAsync(string gameName);
        Task<GameSetup?> GetGameSetupAsync(Guid gameId);
        Task<IList<GameSetup>> GetGameSetupListAsync();
        Task JoinGameSetupAsync(Guid gameId, PlayerSetup playerSetup, Guid versionId);
        Task StartGameAsync(Guid gameId, Guid versionId);
    }
}
