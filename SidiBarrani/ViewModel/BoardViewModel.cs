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
    public class BoardViewModel :
        ViewModelBase
    {
        public Game Game {get;set;}
        
        private string _test;
        public string Test
        {
            get { return _test; }
            set { this.RaiseAndSetIfChanged(ref _test, value); }
        }


        public BoardViewModel()
        {
            var rules = new Rules();
            var playerGroup = Generators.GetExamplePlayerGroup();
            var initialPlayer = playerGroup.GetRandomPlayer();

            Game = new Game(rules, playerGroup);
            var gameResult = Game.ProcessGame();
            Test += gameResult.ToString();
        }
    }
}