using System;
using System.Globalization;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReactiveUI;
using SidiBarrani.Model;
using System.Linq;
using System.Threading;

namespace SidiBarrani.ViewModel
{
    public class BoardViewModel : ReactiveObject
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
        private ObservableAsPropertyHelper<GameRound> _currentGameRound;
        public GameRound CurrentGameRound
        {
            get { return _currentGameRound.Value; }
        }
        private ObservableAsPropertyHelper<RoundResult> _currentRoundResult;
        public RoundResult CurrentRoundResult
        {
            get { return _currentRoundResult.Value; }
        }
        private ObservableAsPropertyHelper<BetStage> _currentBetStage;
        public BetStage CurrentBetStage
        {
            get { return _currentBetStage.Value; }
        }
        private ObservableAsPropertyHelper<BetResult> _currentBetResult;
        public BetResult CurrentBetResult
        {
            get { return _currentBetResult.Value; }
        }
        private ObservableAsPropertyHelper<PlayStage> _currentPlayStage;
        public PlayStage CurrentPlayStage
        {
            get { return _currentPlayStage.Value; }
        }
        private ObservableAsPropertyHelper<PlayResult> _currentPlayResult;
        public PlayResult CurrentPlayResult
        {
            get { return _currentPlayResult.Value; }
        }
        private ObservableAsPropertyHelper<StickRound> _currentStickRound;
        public StickRound CurrentStickRound
        {
            get { return _currentStickRound.Value; }
        }
        private ObservableAsPropertyHelper<StickResult> _currentStickResult;
        public StickResult CurrentStickResult
        {
            get { return _currentStickResult.Value; }
        }
        private ObservableAsPropertyHelper<BetAction> _currentBetAction;
        public BetAction CurrentBetAction
        {
            get { return _currentBetAction.Value; }
        }
        private ObservableAsPropertyHelper<PlayAction> _currentPlayAction;
        public PlayAction CurrentPlayAction
        {
            get { return _currentPlayAction.Value; }
        }
        
        private string _logOutput;
        public string LogOutput
        {
            get { return _logOutput; }
            set { this.RaiseAndSetIfChanged(ref _logOutput, value); }
        }

        public BoardViewModel()
        {
            Rules = new Rules();
            PlayerGroup = Generators.GetExamplePlayerGroup();
            LogOutput = "";
            SetupReactiveProperties();
            SubscribeToReactiveProperties();

            StartGame();
        }

        private void SetupReactiveProperties()
        {
            this.WhenAnyValue(x => x.Game.GameRound)
                .Select(x => x)
                .ToProperty(this, x => x.CurrentGameRound, out _currentGameRound, null);
            this.WhenAnyValue(x => x.Game.RoundResult)
                .Select(x => x)
                .ToProperty(this, x => x.CurrentRoundResult, out _currentRoundResult, null);
            this.WhenAnyValue(x => x.CurrentGameRound.BetStage)
                .Select(x => x)
                .ToProperty(this, x => x.CurrentBetStage, out _currentBetStage, null);
            this.WhenAnyValue(x => x.CurrentGameRound.BetResult)
                .Select(x => x)
                .ToProperty(this, x => x.CurrentBetResult, out _currentBetResult, null);
            this.WhenAnyValue(x => x.CurrentGameRound.PlayStage)
                .Select(x => x)
                .ToProperty(this, x => x.CurrentPlayStage, out _currentPlayStage, null);
            this.WhenAnyValue(x => x.CurrentGameRound.PlayResult)
                .Select(x => x)
                .ToProperty(this, x => x.CurrentPlayResult, out _currentPlayResult, null);
            this.WhenAnyValue(x => x.CurrentPlayStage.CurrentStickRound)
                .Select(x => x)
                .ToProperty(this, x => x.CurrentStickRound, out _currentStickRound, null);
            this.WhenAnyValue(x => x.CurrentPlayStage.StickResult)
                .Select(x => x)
                .ToProperty(this, x => x.CurrentStickResult, out _currentStickResult, null);
            this.WhenAnyValue(x => x.CurrentGameRound.BetAction)
                .Select(x => x)
                .ToProperty(this, x => x.CurrentBetAction, out _currentBetAction, null);
            this.WhenAnyValue(x => x.CurrentGameRound.PlayAction)
                .Select(x => x)
                .ToProperty(this, x => x.CurrentPlayAction, out _currentPlayAction, null);
        }

        private void SubscribeToReactiveProperties()
        {
            // var roundResultSubscription = Game.RoundResultList
            //     .Connect()
            //     .SelectMany(cs => cs)
            //     .Select(c => c.Item.Current)
            //     .Subscribe(s => LogOutput += s.ToString());
            this.WhenAnyValue(x => x.CurrentBetAction)
                .Where(r => r!= null)
                .Subscribe(r => LogOutput += "[Bet] " + r.ToString() + Environment.NewLine);
            this.WhenAnyValue(x => x.CurrentBetResult)
                .Where(r => r!= null)
                .Subscribe(r => LogOutput += "[BetResult] " + r.ToString() + Environment.NewLine);
            this.WhenAnyValue(x => x.CurrentPlayAction)
                .Where(r => r!= null)
                .Subscribe(r => LogOutput += "[Play] " + r.ToString() + Environment.NewLine);
            this.WhenAnyValue(x => x.CurrentStickResult)
                .Where(r => r!= null)
                .Subscribe(r => LogOutput += "[StickResult] " + r.ToString() + Environment.NewLine);
            this.WhenAnyValue(x => x.CurrentPlayResult)
                .Where(r => r!= null)
                .Subscribe(r => LogOutput += "[PlayResult] " + Environment.NewLine + r.ToString() + Environment.NewLine);
            this.WhenAnyValue(x => x.CurrentRoundResult)
                .Where(r => r!= null)
                .Subscribe(r => LogOutput += "[RoundResult] " + Environment.NewLine + r.ToString() + Environment.NewLine);
            this.WhenAnyValue(x => x.GameResult)
                .Where(r => r!= null)
                .Subscribe(r => LogOutput += "[GameResult] " + Environment.NewLine + r.ToString() + Environment.NewLine);
        }

        private async void StartGame()
        {
            Game = new Game(Rules, PlayerGroup);
            GameResult = await Game.ProcessGame();
        }
    }
}