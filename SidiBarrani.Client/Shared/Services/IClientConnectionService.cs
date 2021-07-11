using System;
using System.Threading.Tasks;
using SidiBarrani.Shared.Model.Connection;
using SidiBarrani.Shared.Model.Setup;

namespace SidiBarrani.Client.Shared.Services
{
    public delegate Task Notify();

    public interface IClientConnectionService
    {
        event Notify? GameSetupListChanged;
        event Notify? GameStageChanged;
        event Notify? GameStarted;
        event Notify? GameFinished;
        event Notify? GameContextChanged;
        event Action OnChange;

        bool IsLoggedIn { get; }
        string? UserId { get; }
        string? UserName { get; }
        GameSetup? CurrentGameSetup { get; }
        ConnectionStatus ConnectionStatus { get; }

        Task<string> Login(string userName);
        Task ConnectToHub();
        Task ConnectToGameSetup(GameSetup gameSetup);
        Task DisconnectFromGame(Guid gameId);
        Task<string> GetPlayerConnectionId();
        Task<Guid?> GetCurrentGameId();
    }
}
