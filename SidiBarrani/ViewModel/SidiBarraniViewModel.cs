using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using SidiBarrani.Model;

namespace SidiBarrani.ViewModel
{
    public class SidiBarraniViewModel : ReactiveObject
    {
        private Rules _rules;
        public Rules Rules
        {
            get { return _rules; }
            set { this.RaiseAndSetIfChanged(ref _rules, value); }
        }
        private PlayerGroup _playergroup;
        public PlayerGroup PlayerGroup
        {
            get { return _playergroup; }
            set { this.RaiseAndSetIfChanged(ref _playergroup, value); }
        }
        private Game _game;
        public Game Game
        {
            get { return _game; }
            set { this.RaiseAndSetIfChanged(ref _game, value); }
        }
        private GameResult _gameResult;
        public GameResult GameResult
        {
            get { return _gameResult; }
            set { this.RaiseAndSetIfChanged(ref _gameResult, value); }
        }
        private GameRepresentation _gameRepresentation;
        public GameRepresentation GameRepresentation
        {
            get { return _gameRepresentation; }
            set { this.RaiseAndSetIfChanged(ref _gameRepresentation, value); }
        }
        private LogRepresentation _logRepresentation;
        public LogRepresentation LogRepresentation
        {
            get { return _logRepresentation; }
            set { this.RaiseAndSetIfChanged(ref _logRepresentation, value); }
        }

        public SidiBarraniViewModel()
        {
            PlayerGroup = PlayerGroupFactory.CreatePlayerGroup();
            Rules = new Rules();
            this.WhenAnyValue(x => x.GameResult)
                .Where(r => r != null)
                .Select(r => "[GameResult] " + Environment.NewLine + r.ToString() + Environment.NewLine)
                .Subscribe(output =>
                {
                    if (LogRepresentation != null)
                    {
                        LogRepresentation.LogOutput += output;
                    }
                });

            StartGame();
        }

        private async Task<GameResult> StartGame()
        {
            Game = new Game(Rules, PlayerGroup);
            LogRepresentation = new LogRepresentation(Game, PlayerGroup);
            GameRepresentation = new GameRepresentation(Game, PlayerGroup.GetPlayerList());
            foreach (var player in PlayerGroup.GetPlayerList())
            {
                var isHuman = player == PlayerGroup.Team1.Player1;
                if (isHuman)
                {
                    AttachHumanPlayerInteractions(player, GameRepresentation);
                }
                else
                {
                    AttachComputerPlayerInteractions(player, GameRepresentation);
                }
            }
            var gameResult = await Game.ProcessGame();
            GameResult = gameResult;
            await Player.GetPlayerConfirm(PlayerGroup.GetPlayerList());
            GameRepresentation = null;
            LogRepresentation = null;
            Game = null;
            return gameResult;
        }

        private static void AttachHumanPlayerInteractions(Player player, GameRepresentation gameRepresentation)
        {
            player.AttachObservable(
                gameRepresentation.BetActionObservable,
                gameRepresentation.PlayActionObservable,
                gameRepresentation.ConfirmCommand);
        }

        private static void AttachComputerPlayerInteractions(Player player, GameRepresentation gameRepresentation)
        {
            //TODO: Switch to Observables, e.g. 
            //ReactiveCommand.CreateFromObservable(() => Observable.Delay(TimeSpan.FromSeconds(1));
            var betActionTaskGenerator = new Func<PlayerContext, Task<BetAction>>(playerContext =>
            {
                return Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    return PlayerInteractionsFactory.RandomBetActionGenerator(playerContext);
                });
            });
            var playActionTaskGenerator = new Func<PlayerContext, Task<PlayAction>>(playerContext =>
            {
                return Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    return PlayerInteractionsFactory.RandomPlayActionGenerator(playerContext);
                });
            });
            var confirmTaskGenerator = new Func<PlayerContext, Task>(playerContext =>
            {
                return Task.Run(async () =>
                {
                    await Task.Delay(1000);
                });
            });
            player.AttachTaskGenerator(betActionTaskGenerator, playActionTaskGenerator, confirmTaskGenerator);
        }
    }
}