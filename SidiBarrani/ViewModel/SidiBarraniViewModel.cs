using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using SidiBarraniAi;
using SidiBarraniClient;
using SidiBarraniCommon;
using SidiBarraniCommon.Cache;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;

namespace SidiBarrani.ViewModel
{
    public class SidiBarraniViewModel : ReactiveObject
    {
        private SidiBarraniClientImplementation SidiBarraniClient {get;}

        private ObservableAsPropertyHelper<CommandFactory> _commandFactory;
        public CommandFactory CommandFactory => _commandFactory.Value;

        private ObservableAsPropertyHelper<PlayerGameInfo> _playerGameInfo;
        public PlayerGameInfo PlayerGameInfo => _playerGameInfo.Value;

        private ObservableAsPropertyHelper<GameRepresentation> _gameRepresentation;
        public GameRepresentation GameRepresentation => _gameRepresentation.Value;

        private bool _isAwaitingConfirm;
        public bool IsAwaitingConfirm
        {
            get => _isAwaitingConfirm;
            set => this.RaiseAndSetIfChanged(ref _isAwaitingConfirm, value);
        }
        public ReactiveCommand<Unit, bool> ConfirmCommand {get;}

        private async Task<bool> GetConfirmTask()
        {
            IsAwaitingConfirm = true;
            var result = await ConfirmCommand.FirstAsync();
            IsAwaitingConfirm = false;
            return result;
        }

        public SidiBarraniViewModel(ISidiBarraniServerApi sidiBarraniServerApi)
        {
            ConfirmCommand = ReactiveCommand.Create<bool>(() => true);
            SidiBarraniClient = new SidiBarraniClientImplementation(sidiBarraniServerApi, GetConfirmTask);

            this.WhenAnyValue(x => x.SidiBarraniClient.PlayerGameInfo)
                .ToProperty(this, x => x.PlayerGameInfo, out _playerGameInfo, null);
            this.WhenAnyValue(
                x => x.SidiBarraniClient.GameInfo,
                x => x.SidiBarraniClient.PlayerInfo,
                (gameInfo, playerInfo) => (gameInfo != null && playerInfo != null)
                    ? new CommandFactory(
                        sidiBarraniServerApi,
                        gameInfo,
                        playerInfo,
                        new ActionCache(gameInfo.Rules))
                    : null)
                .ToProperty(this, x => x.CommandFactory, out _commandFactory, null);
            this.WhenAnyValue(
                x => x.CommandFactory,
                x => x.PlayerGameInfo,
                (f, g) => (f != null && g != null)
                    ? new GameRepresentation(f, g)
                    : null)
                .ToProperty(this, x => x.GameRepresentation, out _gameRepresentation, null);

            StartGame(sidiBarraniServerApi);
        }

        private void StartGame(ISidiBarraniServerApi sidiBarraniServerApi)
        {
            var rules = new Rules
            {
                EndScore = 100
            };
            SidiBarraniClient.OpenGame(rules, "Game1", "TeamA", "TeamB");
            SidiBarraniClient.ConnectToGame(SidiBarraniClient.GameInfo, "Player1");

            // Simulate game start with 3 other clients
            var client2 = new SidiBarraniRandomClient(sidiBarraniServerApi,1000);
            var client3 = new SidiBarraniRandomClient(sidiBarraniServerApi,1000);
            var client4 = new SidiBarraniRandomClient(sidiBarraniServerApi,1000);
            var gameId = SidiBarraniClient.GameInfo.GameId;
            client2.ConnectToGame(gameId, "Player2");
            client3.ConnectToGame(gameId, "Player3");
            client4.ConnectToGame(gameId, "Player4");
            SidiBarraniClient.StartGame();
        }
    }
}