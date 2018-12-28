using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace SidiBarrani.Model
{
    public class Game : ReactiveObject
    {
        private Game() { }
        public Game(Rules rules, PlayerGroup playerGroup)
        {
            Rules = rules;
            PlayerGroup = playerGroup;
            RoundResultSourceList = new SourceList<RoundResult>();
            RoundResultList = RoundResultSourceList.AsObservableList();
        }
        private Rules Rules {get;}
        private PlayerGroup PlayerGroup {get;}
        private SourceList<RoundResult> RoundResultSourceList {get;}
        public IObservableList<RoundResult> RoundResultList {get;}

        private GameRound _gameRound;
        public GameRound GameRound
        {
            get { return _gameRound; }
            private set { this.RaiseAndSetIfChanged(ref _gameRound, value); }
        }
        private RoundResult _roundResult;
        public RoundResult RoundResult
        {
            get { return _roundResult; }
            private set { this.RaiseAndSetIfChanged(ref _roundResult, value); }
        }

        public async Task<GameResult> ProcessGame()
        {
            var initialPlayer = PlayerGroup.GetRandomPlayer();
            var gameResult = GetGameResult(Rules, PlayerGroup, RoundResultSourceList.Items.ToList());
            while (gameResult == null)
            {
                GameRound = new GameRound(Rules, PlayerGroup, initialPlayer);
                RoundResult = await GameRound.ProcessRound();
                await Player.GetPlayerConfirm(PlayerGroup.GetPlayerList());
                GameRound = null;
                RoundResultSourceList.Add(RoundResult);
                initialPlayer = PlayerGroup.GetNextPlayer(initialPlayer);
                gameResult = GetGameResult(Rules, PlayerGroup, RoundResultSourceList.Items.ToList());
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