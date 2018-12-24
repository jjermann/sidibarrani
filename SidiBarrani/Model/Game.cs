using System;
using System.Collections.Generic;
using System.Linq;

namespace SidiBarrani.Model
{
    public class Game
    {
        private Game() { }
        public Game(Rules rules, PlayerGroup playerGroup)
        {
            Rules = rules;
            PlayerGroup = playerGroup;
        }
        Rules Rules {get;}
        PlayerGroup PlayerGroup {get;}

        public GameResult PlayGame()
        {
            var initialPlayer = PlayerGroup.GetRandomPlayer();
            var gameResult = GetGameResult();
            while (gameResult == null)
            {
                var gameRound = new GameRound(Rules, PlayerGroup, initialPlayer);
                var roundResult = gameRound.PlayRound();
                initialPlayer = PlayerGroup.GetNextPlayer(initialPlayer);
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