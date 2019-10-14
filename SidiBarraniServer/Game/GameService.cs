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
        private Action ConfirmAction {get;set;}
        public IDictionary<string, ISidiBarraniClientApi> ClientApiDictionary {get;set;} = new Dictionary<string, ISidiBarraniClientApi>();
        public GameStage GameStage {get;set;}
        private bool IsBusy {get;set;}
        private object _lock = new object();

        public GameService(GameInfo gameInfo)
        {
            GameInfo = gameInfo;
        }

        private void RunBusyAction(Action action)
        {
            IsBusy = true;
            Task.Run(() => {
                action();
                IsBusy = false;
            });
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

            RunBusyAction(() => 
            {
                GameStage = new GameStage(GameInfo.Rules, PlayerGroupInfo, ConfirmAction);
                UpdatePlayers();
            });
            return true;
        }

        public bool ProcessAction(ActionBase action)
        {
            if (IsBusy)
            {
                return false;
            }
            if (GameStage == null)
            {
                return false;
            }
            var validPlayerActions = GetValidActionIdList(action.PlayerInfo.PlayerId);
            if (!validPlayerActions.Contains(action.GetActionId()))
            {
                return false;
            }
            RunBusyAction(() =>
            {
                GameStage.ProcessAction(action);
                UpdatePlayers();
            });
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

        private IList<int> GetValidActionIdList(string playerId)
        {
            if (GameStage == null)
            {
                return new List<int>();
            }
            return GameStage.GetValidActionIdList(playerId);
        }
    }
}