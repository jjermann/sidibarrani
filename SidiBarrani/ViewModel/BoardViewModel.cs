using System;
using System.Globalization;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReactiveUI;
using SidiBarrani.Model;
using System.Linq;

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
            var playerGroup = GetPlayerGroup();
            var initialPlayer = playerGroup.GetRandomPlayer();

            //For testing
            var betStage = new BetStage(rules, playerGroup, initialPlayer);
            var betResult = betStage.GetBetResult();
            while (betResult == null) {
                var validActions = betStage.GetValidBetActions();
                var randomAction = validActions.OrderBy(a => Guid.NewGuid()).FirstOrDefault();
                betStage.AddBetActionAndProgress(randomAction);
                Test = betStage.ToString();
                betResult = betStage.GetBetResult();
            }
            Test = betStage.ToString();

            // Game = new Game(rules, playerGroup);
            // var gameResult = Game.PlayGame();
        }

        public PlayerGroup GetPlayerGroup()
        {
            var player1 = new Player
            {
                Name = "Player1"
            };
            var player2 = new Player
            {
                Name = "Player2"
            };
            var team1 = new Team {
                Player1 = player1,
                Player2 = player2 
            };
            player1.Team = team1;
            player2.Team = team1;

            var player3 = new Player
            {
                Name = "Player3"
            };
            var player4 = new Player
            {
                Name = "Player4"
            };
            var team2 = new Team {
                Player1 = player3,
                Player2 = player4
            };
            player3.Team = team2;
            player4.Team = team2;
            var playerGroup = new PlayerGroup(team1, team2);
            return playerGroup;
        }
    }
}