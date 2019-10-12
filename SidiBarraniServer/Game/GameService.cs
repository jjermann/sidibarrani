using System;
using System.Collections.Generic;
using System.Linq;
using SidiBarraniCommon;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Info;

namespace SidiBarraniServer.Game
{
    public class GameService
    {
        public GameInfo GameInfo {get;set;}
        public PlayerGroupInfo PlayerGroupInfo {get;set;}
        public IDictionary<string, ISidiBarraniClientApi> ClientApiDictionary {get;set;} = new Dictionary<string, ISidiBarraniClientApi>();
        public IList<GameRound> GameRoundList {get;set;} = new List<GameRound>();
        public GameRound CurrentGameRound => GameRoundList.LastOrDefault();
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
            var gameRound = new GameRound();
            var firstPlayer = GetRandomPlayer();
            var result = gameRound.StartRound(firstPlayer);
            if (!result)
            {
                return false;
            }
            IsActive = true;
            GameRoundList.Add(gameRound);
            return result;
        }
        public bool ProcessAction(ActionBase action)
        {
            var validPlayerActions = GetValidActionIdList(action.PlayerInfo.PlayerId);
            if (!validPlayerActions.Contains(action.GetActionId()))
            {
                return false;
            }
            CurrentGameRound.ProcessAction(action);
            foreach (var player in PlayerGroupInfo.GetPlayerList())
            {
                var playerGameInfo = GetPlayerGameInfo(player.PlayerId);
                ClientApiDictionary[player.PlayerId].SetPlayerGameInfo(playerGameInfo);
            }
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