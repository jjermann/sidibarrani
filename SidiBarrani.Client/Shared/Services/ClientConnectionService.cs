using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using SidiBarrani.Shared.Constants;
using SidiBarrani.Shared.Model.Connection;
using SidiBarrani.Shared.Services;

namespace SidiBarrani.Client.Shared.Services
{
    public class ClientConnectionService: IClientConnectionService, IDisposable
    {
        private readonly HubConnection _hubConnection;
        private readonly IList<IDisposable> _disposableList;

        public ConnectionStatus ConnectionStatus { get; private set; } = ConnectionStatus.Closed;
        private readonly TaskCompletionSource<string> _connectionIdCompletionSource = new();

        public ClientConnectionService(string baseAddress)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithAutomaticReconnect()
                .WithUrl(baseAddress + ApiRouteConstants.NotificationHub)
                .Build();
            _disposableList = new List<IDisposable>
            {
                _hubConnection.On(ConnectionEvent.GameStageListChanged.ToString(), () => GameSetupListChanged?.Invoke()),
                _hubConnection.On(ConnectionEvent.GameStarted.ToString(), () => GameStarted?.Invoke()),
                _hubConnection.On(ConnectionEvent.GameFinished.ToString(), () => GameFinished?.Invoke()),
                _hubConnection.On(ConnectionEvent.GameStageChanged.ToString(), () => GameStageChanged?.Invoke()),
                _hubConnection.On(ConnectionEvent.GameContextChanged.ToString(), () => GameContextChanged?.Invoke())
            };
        }

        public async Task ConnectToHub()
        {
            if (ConnectionStatus != ConnectionStatus.Closed)
            {
                return;
            }

            await _hubConnection.StartAsync();
            ConnectionStatus = ConnectionStatus.Connected;
            _connectionIdCompletionSource.SetResult(_hubConnection.ConnectionId);

            _hubConnection.Closed += async _ =>
            {
                ConnectionStatus = ConnectionStatus.Reconnecting;
                await _hubConnection.StartAsync();
                ConnectionStatus = ConnectionStatus.Connected;
                _connectionIdCompletionSource.SetResult(_hubConnection.ConnectionId);
            };
            _hubConnection.Reconnecting += async _ =>
            {
                await Task.Run(() =>
                {
                    ConnectionStatus = ConnectionStatus.Reconnecting;
                });
            };
            _hubConnection.Reconnected += async connectionId =>
            {
                await Task.Run(() =>
                {
                    ConnectionStatus = ConnectionStatus.Connected;
                });
                _connectionIdCompletionSource.SetResult(connectionId);
            };
        }

        public event Notify? GameSetupListChanged;
        public event Notify? GameStageChanged;
        public event Notify? GameStarted;
        public event Notify? GameFinished;
        public event Notify? GameContextChanged;

        public async Task<string> GetPlayerConnectionId()
        {
            await ConnectToHub();
            return await _connectionIdCompletionSource.Task;
        }

        public void Dispose()
        {
            foreach (var disposable in _disposableList)
            {
                disposable.Dispose();
            }
        }
    }
}
