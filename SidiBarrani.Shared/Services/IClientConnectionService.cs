using System.Threading.Tasks;
using SidiBarrani.Shared.Model.Connection;

namespace SidiBarrani.Shared.Services
{
    public delegate Task Notify();

    public interface IClientConnectionService
    {
        event Notify? GameSetupListChanged;
        event Notify? GameStageChanged;
        event Notify? GameStarted;
        event Notify? GameFinished;
        event Notify? GameContextChanged;

        Task ConnectToHub();
        Task<string> GetPlayerConnectionId();
        ConnectionStatus ConnectionStatus { get; }
    }
}
