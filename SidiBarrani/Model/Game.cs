using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SidiBarrani.Model
{
    public class Game
    {
        private Game() { }
        public Game(Rules rules, PlayerGroup playerGroup)
        {
            Rules = rules;
            PlayerGroup = playerGroup;
            RoundResultsList = new List<RoundResult>();
        }
        private Rules Rules {get;}
        private PlayerGroup PlayerGroup {get;}
        private IList<RoundResult> RoundResultsList {get;}

        public async Task<GameResult> ProcessGame()
        {
            var initialPlayer = PlayerGroup.GetRandomPlayer();
            var gameResult = GetGameResult(Rules, PlayerGroup, RoundResultsList);
            while (gameResult == null)
            {
                var gameRound = new GameRound(Rules, PlayerGroup, initialPlayer);
                var roundResult = await gameRound.ProcessRound();
                RoundResultsList.Add(roundResult);
                initialPlayer = PlayerGroup.GetNextPlayer(initialPlayer);
                gameResult = GetGameResult(Rules, PlayerGroup, RoundResultsList);
            }
            return gameResult;
        }

        public static GameResult GetGameResult(Rules rules, PlayerGroup playerGroup, IList<RoundResult> roundResultList) {
            var team1FinalScore = roundResultList.Sum(r => r.Team1FinalScore);
            var team2FinalScore = roundResultList.Sum(r => r.Team2FinalScore);
            var endScore = rules.EndScore;
            var hasEnded = team1FinalScore >= endScore || team2FinalScore >= endScore;
            if (!hasEnded)
            {
                return null;
            }

            Team winner;
            var bothOverEndScore = team1FinalScore >= endScore && team2FinalScore >= endScore;
            if (bothOverEndScore)
            {
                winner = roundResultList
                    .Last()
                    .WinningTeam;
            }
            else
            {
                winner = team1FinalScore > team2FinalScore
                    ? playerGroup.Team1
                    : playerGroup.Team2;
            }
            var gameResult = new GameResult
            {
                Winner = winner,
                PlayerGroup = playerGroup,
                Team1FinalScore = team1FinalScore,
                Team2FinalScore = team2FinalScore
            };
            return gameResult;
        }
    }
}