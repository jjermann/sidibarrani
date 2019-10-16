using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;
using SidiBarraniClient;
using SidiBarraniCommon;
using SidiBarraniCommon.Cache;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;
using SidiBarraniCommon.Result;

namespace SidiBarrani.ViewModel
{
    public class SidiBarraniViewModel : ReactiveObject
    {
        private SidiBarraniClientImplementation SidiBarraniClient {get;}

        private ObservableAsPropertyHelper<CommandFactory> _commandFactory;
        public CommandFactory CommandFactory
        {
            get { return _commandFactory.Value; }
        }

        private ObservableAsPropertyHelper<PlayerGameInfo> _playerGameInfo;
        public PlayerGameInfo PlayerGameInfo
        {
            get { return _playerGameInfo.Value; }
        }

        private ObservableAsPropertyHelper<GameRepresentation> _gameRepresentation;
        public GameRepresentation GameRepresentation
        {
            get { return _gameRepresentation.Value; }
        }

        public SidiBarraniViewModel(ISidiBarraniServerApi sidiBarraniServerApi)
        {
            SidiBarraniClient = new SidiBarraniClientImplementation(sidiBarraniServerApi);
            var rules = new Rules
            {
                EndScore = 100
            };
            SidiBarraniClient.OpenGame(rules, "Game1", "TeamA", "TeamB");
            SidiBarraniClient.ConnectToGame(SidiBarraniClient.GameInfo, "Player1");

            this.WhenAnyValue(x => x.SidiBarraniClient.PlayerGameInfo)
                .ToProperty(this, x => x.PlayerGameInfo, out _playerGameInfo, null);
            this.WhenAnyValue(
                x => x.SidiBarraniClient.GameInfo,
                x => x.SidiBarraniClient.PlayerInfo,
                (gameInfo, playerInfo) => new CommandFactory(
                    sidiBarraniServerApi,
                    gameInfo,
                    playerInfo,
                    new ActionCache(gameInfo.Rules)))
                .ToProperty(this, x => x.CommandFactory, out _commandFactory, null);
            this.WhenAnyValue(x => x.CommandFactory, x => x.PlayerGameInfo, (f, g) => new GameRepresentation(f, g))
                .ToProperty(this, x => x.GameRepresentation, out _gameRepresentation, null);

            // Simulate game start with 3 other clients
            var client2 = new SidiBarraniClientImplementation(sidiBarraniServerApi);
            var client3 = new SidiBarraniClientImplementation(sidiBarraniServerApi);
            var client4 = new SidiBarraniClientImplementation(sidiBarraniServerApi);
            client2.RefreshOpenGames();
            client3.RefreshOpenGames();
            client4.RefreshOpenGames();
            client2.ConnectToGame(client2.OpenGameList.First(), "Player2");
            client3.ConnectToGame(client3.OpenGameList.First(), "Player3");
            client4.ConnectToGame(client4.OpenGameList.First(), "Player4");

            Task.Run(() => RunGame(SidiBarraniClient, client2, client3, client4));
        }

        private static GameResult RunGame(
            SidiBarraniClientImplementation client1,
            SidiBarraniClientImplementation client2,
            SidiBarraniClientImplementation client3,
            SidiBarraniClientImplementation client4)
        {
            client1.StartGame();
            Thread.Sleep(500);
            var clientList = new List<SidiBarraniClientImplementation> {client1, client2, client3, client4};

            var random = new Random();
            var gameResult = clientList.First().PlayerGameInfo?.GameStageInfo?.GameResult;
            while (gameResult == null)
            {
                foreach (var client in clientList)
                {
                    var validActions = client.GetValidActions();
                    if (validActions.Any())
                    {
                        var randomAction = validActions[random.Next(validActions.Count-1)];
                        if (client.ProcessAction(randomAction))
                        {
                            Thread.Sleep(500);
                        }
                    }
                }
                gameResult = clientList.First().PlayerGameInfo?.GameStageInfo?.GameResult;
            }
            return gameResult;
        }
    }
}