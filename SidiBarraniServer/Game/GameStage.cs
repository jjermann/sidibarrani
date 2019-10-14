using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using SidiBarraniCommon;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;
using SidiBarraniCommon.Result;

namespace SidiBarraniServer.Game
{
    public class GameStage
    {
        private Rules Rules {get;}
        private PlayerGroupInfo PlayerGroupInfo {get;}
        private Action ConfirmAction {get;}

        public IList<GameRound> GameRoundList {get;set;} = new List<GameRound>();
        public GameRound CurrentGameRound => GameRoundList.LastOrDefault();
        public PlayerInfo CurrentPlayer {get;set;}
        public GameResult GameResult {get;set;}
        private Random _random = new Random();

        public GameStage(
            Rules rules,
            PlayerGroupInfo playerGroupInfo,
            Action confirmAction)
        {
            Rules = rules;
            PlayerGroupInfo = playerGroupInfo;
            ConfirmAction = confirmAction;
            CurrentPlayer = GetRandomPlayer();
            var deck = CardPile.CreateDeckPile();
            var gameRound = new GameRound(Rules, PlayerGroupInfo, ConfirmAction, CurrentPlayer, deck);
            GameRoundList.Add(gameRound);
        }

        private PlayerInfo GetRandomPlayer()
        {
            var playerList = PlayerGroupInfo.GetPlayerList();
            if (!playerList.Any())
            {
                return null;
            }
            var randomIndex = _random.Next(playerList.Count-1);
            return playerList[randomIndex];
        }

        public void ProcessAction(ActionBase action)
        {
            Log.Information(action.ToString());
            CurrentGameRound.ProcessAction(action);
            if (CurrentGameRound.RoundResult != null)
            {
                Log.Information(CurrentGameRound.RoundResult.ToString());
                ConfirmAction?.Invoke();
                GameResult = GetGameResult();
                if (GameResult == null)
                {
                    CurrentPlayer = PlayerGroupInfo.GetNextPlayer(CurrentPlayer.PlayerId);
                    var deck = CardPile.CreateDeckPile();
                    var gameRound = new GameRound(Rules, PlayerGroupInfo, ConfirmAction, CurrentPlayer, deck);
                    GameRoundList.Add(gameRound);
                }
                else {
                    Log.Information(GameResult.ToString());
                }
            }
        }

        private GameResult GetGameResult() {
            var team1FinalScore = GameRoundList
                .Where(g => g.RoundResult != null)
                .Sum(g => g.RoundResult.Team1FinalScore);
            var team2FinalScore = GameRoundList
                .Where(g => g.RoundResult != null)
                .Sum(g => g.RoundResult.Team2FinalScore);
            // TODO: Maybe also count results from the current round to end early
            var endScore = Rules.EndScore;
            var hasEnded = team1FinalScore >= endScore || team2FinalScore >= endScore;
            if (!hasEnded)
            {
                return null;
            }

            TeamInfo winner;
            var bothOverEndScore = team1FinalScore >= endScore && team2FinalScore >= endScore;
            if (bothOverEndScore)
            {
                winner = GameRoundList
                    .Where(g => g.RoundResult != null)
                    .Last()
                    .RoundResult
                    .WinningTeam;
            }
            else
            {
                winner = team1FinalScore > team2FinalScore
                    ? PlayerGroupInfo.Team1
                    : PlayerGroupInfo.Team2;
            }
            var gameResult = new GameResult
            {
                Winner = winner,
                PlayerGroupInfo = PlayerGroupInfo,
                Team1FinalScore = team1FinalScore,
                Team2FinalScore = team2FinalScore
            };
            return gameResult;
        }

        public IList<int> GetValidActionIdList(string playerId)
        {
            if (CurrentGameRound == null)
            {
                return new List<int>();
            }
            return CurrentGameRound.GetValidActionIdList(playerId);
        }
    }
}