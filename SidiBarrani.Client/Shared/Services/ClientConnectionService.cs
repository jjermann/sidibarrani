using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using SidiBarrani.Shared.Constants;
using SidiBarrani.Shared.Model.Connection;
using SidiBarrani.Shared.Model.Setup;

namespace SidiBarrani.Client.Shared.Services
{
    public class ClientConnectionService: IClientConnectionService, IDisposable
    {
        private readonly HubConnection _hubConnection;
        private readonly IList<IDisposable> _disposableList;
        private TaskCompletionSource<string> _connectionIdCompletionSource = new();
        private TaskCompletionSource<Guid?> _gameIdCompletionSource = new();

        public ConnectionStatus ConnectionStatus { get; private set; } = ConnectionStatus.Closed;
        private Guid? CurrentGameId { get; set; }
        public GameSetup? CurrentGameSetup { get; private set; }
        public bool IsLoggedIn => UserId != null;
        public string? UserId { get; private set; }
        public string? UserName { get; private set; }

        public event Notify? GameSetupListChanged;
        public event Notify? GameStageChanged;
        public event Notify? GameStarted;
        public event Notify? GameFinished;
        public event Notify? GameContextChanged;
        public event Action? OnChange;

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
            _gameIdCompletionSource.SetResult(null);
        }

        private void StateHasChanged() => OnChange?.Invoke();

        public async Task ConnectToGameSetup(GameSetup gameSetup)
        {
            await ConnectToGameGroup(gameSetup.GameId);
            CurrentGameSetup = gameSetup;
            StateHasChanged();
        }

        private async Task ConnectToGameGroup(Guid gameId)
        {
            var currentGameId = await GetCurrentGameId();

            if (currentGameId.HasValue)
            {
                if (currentGameId.Value == gameId)
                {
                    return;
                }

                await DisconnectFromGame(currentGameId.Value);
            }

            _gameIdCompletionSource = new TaskCompletionSource<Guid?>();
            await GetPlayerConnectionId();
            await _hubConnection.InvokeAsync(ConnectionEvent.JoinGroup.ToString(), gameId.ToString());
            CurrentGameId = gameId;
            _gameIdCompletionSource.SetResult(gameId);
            StateHasChanged();
        }

        public async Task DisconnectFromGame(Guid gameId)
        {
            var currentGameId = await GetCurrentGameId();

            if (!currentGameId.HasValue)
            {
                return;
            }

            _gameIdCompletionSource = new TaskCompletionSource<Guid?>();
            await GetPlayerConnectionId();
            await _hubConnection.InvokeAsync(ConnectionEvent.LeaveGroup.ToString(), gameId.ToString());
            CurrentGameId = null;
            _gameIdCompletionSource.SetResult(null);
            StateHasChanged();
        }

        //public async Task DisconnectFromHub()
        //{
        //    await _hubConnection.StopAsync();
        //    ConnectionStatus = ConnectionStatus.Closed;
        //}

        public async Task ConnectToHub()
        {
            if (ConnectionStatus != ConnectionStatus.Closed)
            {
                return;
            }

            if (_connectionIdCompletionSource.Task.IsCompleted)
            {
                _connectionIdCompletionSource = new TaskCompletionSource<string>();
            }

            await _hubConnection.StartAsync();
            await SyncConnectedState(_hubConnection.ConnectionId);

            //_hubConnection.Closed += async _ =>
            //{
            //    if (_connectionIdCompletionSource.Task.IsCompleted)
            //    {
            //        _connectionIdCompletionSource = new TaskCompletionSource<string>();
            //    }
            //    ConnectionStatus = ConnectionStatus.Reconnecting;
            //    await _hubConnection.StartAsync();
            //    await SyncConnectedState(_hubConnection.ConnectionId);
            //};
            _hubConnection.Reconnecting += async _ =>
            {
                if (_connectionIdCompletionSource.Task.IsCompleted)
                {
                    _connectionIdCompletionSource = new TaskCompletionSource<string>();
                }

                await Task.Run(() =>
                {
                    ConnectionStatus = ConnectionStatus.Reconnecting;
                    StateHasChanged();
                });
            };
            _hubConnection.Reconnected += SyncConnectedState;
        }

        private async Task SyncConnectedState(string connectionId)
        {
            if (!_gameIdCompletionSource.Task.IsCompleted)
            {
                _gameIdCompletionSource.SetResult(null);
            }

            if (CurrentGameId.HasValue)
            {
                _gameIdCompletionSource = new TaskCompletionSource<Guid?>();
                await _hubConnection.InvokeAsync(ConnectionEvent.JoinGroup.ToString(), CurrentGameId.Value.ToString());
                _gameIdCompletionSource.SetResult(CurrentGameId.Value);
            }

            ConnectionStatus = ConnectionStatus.Connected;
            _connectionIdCompletionSource.SetResult(connectionId);
            StateHasChanged();
        }

        public async Task<string> GetPlayerConnectionId()
        {
            await ConnectToHub();
            return await _connectionIdCompletionSource.Task;
        }

        public async Task<Guid?> GetCurrentGameId()
        {
            await ConnectToHub();
            return await _gameIdCompletionSource.Task;
        }

        public void Dispose()
        {
            foreach (var disposable in _disposableList)
            {
                disposable.Dispose();
            }
        }

        public async Task<string> Login(string userName)
        {
            if (IsLoggedIn)
            {
                throw new InvalidOperationException();
            }

            UserName = userName;
            UserId = Guid.NewGuid().ToString();

            await ConnectToHub();
            StateHasChanged();

            return UserId;
        }
    }
}
