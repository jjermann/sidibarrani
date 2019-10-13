using System;
using System.Collections.Generic;
using System.Linq;
using SidiBarraniCommon;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;
using SidiBarraniCommon.Result;

namespace SidiBarraniServer.Game
{
    public class GameService
    {
        public GameInfo GameInfo {get;set;}
        public PlayerGroupInfo PlayerGroupInfo {get;set;}
        public IDictionary<string, ISidiBarraniClientApi> ClientApiDictionary {get;set;} = new Dictionary<string, ISidiBarraniClientApi>();
        public IList<GameRound> GameRoundList {get;set;} = new List<GameRound>();
        public GameRound CurrentGameRound => GameRoundList.LastOrDefault();
        public PlayerInfo CurrentPlayer {get;set;}
        public GameResult GameResult {get;set;}
        public bool IsActive {get;set;}

        private Random _random = new Random();

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

        private void UpdatePlayers()
        {
            foreach (var player in PlayerGroupInfo.GetPlayerList())
            {
                var playerGameInfo = GetPlayerGameInfo(player.PlayerId);
                ClientApiDictionary[player.PlayerId].SetPlayerGameInfo(playerGameInfo);
            }
        }

        public bool StartGame()
        {
            if (IsActive)
            {
                return false;
            }
            if (PlayerGroupInfo.GetPlayerList().Count != 4)
            {
                return false;
            }
            CurrentPlayer = GetRandomPlayer();
            IsActive = true;

            var deck = CardPile.CreateDeckPile();
            var gameRound = new GameRound(GameInfo.Rules, GameInfo.PlayerGroupInfo, CurrentPlayer, deck);
            GameRoundList.Add(gameRound);
            UpdatePlayers();
            return true;
        }

        public bool ProcessAction(ActionBase action)
        {
            var validPlayerActions = GetValidActionIdList(action.PlayerInfo.PlayerId);
            if (!validPlayerActions.Contains(action.GetActionId()))
            {
                return false;
            }
            CurrentGameRound.ProcessAction(action);
            if (CurrentGameRound.RoundResult != null)
            {
                GameResult = GetGameResult();
                if (GameResult == null)
                {
                    CurrentPlayer = PlayerGroupInfo.GetNextPlayer(CurrentPlayer.PlayerId);
                    var deck = CardPile.CreateDeckPile();
                    var gameRound = new GameRound(GameInfo.Rules, GameInfo.PlayerGroupInfo, CurrentPlayer, deck);
                    GameRoundList.Add(gameRound);
                }
            }
            UpdatePlayers();
            return true;
        }

        private PlayerGameInfo GetPlayerGameInfo(string playerId)
        {
            return new PlayerGameInfo
            {
                ValidActionList = GetValidActionIdList(playerId)
                    .Select(id => new ActionInfo
                    {
                        GameId = GameInfo.GameId,
                        PlayerId = playerId,
                        ActionId = id
                    })
                    .ToList()
            };
        }

        private GameResult GetGameResult() {
            var team1FinalScore = GameRoundList
                .Where(g => g.RoundResult != null)
                .Sum(g => g.RoundResult.Team1FinalScore);
            var team2FinalScore = GameRoundList
                .Where(g => g.RoundResult != null)
                .Sum(g => g.RoundResult.Team2FinalScore);
            // TODO: Maybe also count results from the current round to end early
            var endScore = GameInfo.Rules.EndScore;
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
            if (!IsActive || CurrentGameRound == null)
            {
                return new List<int>();
            }
            return CurrentGameRound.GetValidActionIdList(playerId);
        }
    }
}