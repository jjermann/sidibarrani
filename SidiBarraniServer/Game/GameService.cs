using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SidiBarraniCommon;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Info;

namespace SidiBarraniServer.Game
{
    public class GameService
    {
        public GameInfo GameInfo {get;set;}
        public PlayerGroupInfo PlayerGroupInfo => GameInfo.PlayerGroupInfo;
        public IDictionary<string, ISidiBarraniClientApi> ClientApiDictionary {get;set;} = new Dictionary<string, ISidiBarraniClientApi>();
        public GameStage GameStage {get;set;}
        private bool IsBusy {get;set;}
        private object _lock = new object();

        public GameService(GameInfo gameInfo)
        {
            GameInfo = gameInfo;
        }

        public bool StartGame()
        {
            if (GameStage != null)
            {
                return false;
            }
            if (PlayerGroupInfo.GetPlayerList().Count != 4)
            {
                return false;
            }

            GameStage = new GameStage(GameInfo.Rules, PlayerGroupInfo);
            UpdatePlayers();
            return true;
        }

        public bool ProcessAction(ActionBase action)
        {
            if (GameStage == null)
            {
                return false;
            }
            var validPlayerActions = GetValidActionIdList(action.PlayerInfo.PlayerId);
            if (!validPlayerActions.Contains(action.GetActionId()))
            {
                return false;
            }
            GameStage.ProcessAction(action);
            UpdatePlayers();
            return true;
        }
        
        private void UpdatePlayers()
        {
            foreach (var player in PlayerGroupInfo.GetPlayerList())
            {
                var playerGameInfo = GetPlayerGameInfo(player.PlayerId);
                ClientApiDictionary[player.PlayerId].SetPlayerGameInfo(playerGameInfo);
            }
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
            if (GameStage == null)
            {
                return new List<int>();
            }
            return GameStage.GetValidActionIdList(playerId);
        }
    }
}