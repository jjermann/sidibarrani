using System;
using System.Globalization;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReactiveUI;
using SidiBarrani.Model;

namespace SidiBarrani.ViewModel
{
    public class BoardViewModel :
        ViewModelBase
    {
        public Game Game {get;set;}

        public BoardViewModel()
        {
            var player1 = new Player();
            var player2 = new Player();
            var team1 = new Team {
                Player1 = player1,
                Player2 = player2 
            };
            player1.Team = team1;
            player2.Team = team1;

            var player3 = new Player();
            var player4 = new Player();
            var team2 = new Team {
                Player1 = player3,
                Player2 = player4
            };
            player3.Team = team2;
            player4.Team = team2;
            
            Game = new Game(team1, team2);
            // var gameResult = Game.PlayGame();
        }
    }
}