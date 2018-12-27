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

        private string _test;
        public string Test
        {
            get { return _test; }
            set { this.RaiseAndSetIfChanged(ref _test, value); }
        }

        public BoardViewModel()
        {
            Rules = new Rules();
            PlayerGroup = Generators.GetExamplePlayerGroup();
            StartGame();
        }

        private async void StartGame()
        {
            Game = new Game(Rules, PlayerGroup);
            var roundResultSubscription = Game.RoundResultList
                .Connect()
                .SelectMany(cs => cs)
                .Select(c => c.Item.Current)
                .Subscribe(s => Test += s.ToString());
            GameResult = await Game.ProcessGame();
            Test += GameResult.ToString();
        }
    }
}