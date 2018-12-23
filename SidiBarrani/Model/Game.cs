using System;
using System.Collections.Generic;
using System.Linq;

namespace SidiBarrani.Model
{
    public class Game
    {
        private Game() { }
        public Game(Team team1, Team team2)
        {
            Team1 = team1;
            Team2 = team2;
        }
        Team Team1 {get;set;}
        Team Team2 {get;set;}

        public IList<Player> GetPlayerList(Player initialPlayer = null)
        {
            var playerList = new List<Player>
            {   
                Team1.Player1,
                Team2.Player1,
                Team1.Player2,
                Team2.Player2
            };
            if (initialPlayer == null)
            {
                initialPlayer = Team1.Player1;
            }
            while (playerList.First() != initialPlayer)
            {
                var tmpFirstPlayer = playerList.First();
                playerList.RemoveAt(0);
                playerList.Add(tmpFirstPlayer);
            }
            return playerList;
        }

        public GameResult PlayGame()
        {
            var currentPlayer = GetPlayerList().OrderBy(p => Guid.NewGuid()).First();
            var gameResult = GetGameResult();
            while (gameResult == null)
            {
                var gameRound = new GameRound();
                var roundResult = gameRound.PlayRound();
                // update game state using roundResult
                gameResult = GetGameResult();
            }
            return gameResult;
        }

        public GameResult GetGameResult() {
            //Update GameResult
            return null;
        }
    }
}