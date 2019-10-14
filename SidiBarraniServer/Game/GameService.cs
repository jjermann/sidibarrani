using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using SidiBarraniCommon;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;

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
            ConfirmAction = () => {
                Parallel.ForEach(ClientApiDictionary.Values, client =>
                {
                    client.RequestConfirm();
                });
            };
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
                try {
                    GameStage = new GameStage(GameInfo.Rules, PlayerGroupInfo, ConfirmAction);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception in StartGame: {e.Message}", e);
                    throw;
                }
                try {
                    UpdatePlayers();
                }
                catch (Exception e)
                {
                    Log.Error($"Exception in UpdatePlayers: {e.Message}", e);
                    throw;
                }
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

            try {
                var validPlayerActions = GetValidActionIdList(action.PlayerInfo.PlayerId);
                if (!validPlayerActions.Contains(action.GetActionId()))
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception in GetValidActionIdList of ProcessAction: {e.Message}", e);
                throw;
            }

            RunBusyAction(() =>
            {
                try {
                    GameStage.ProcessAction(action);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception in ProcessAction: {e.Message}", e);
                    throw;
                }
                try {
                    UpdatePlayers();
                }
                catch (Exception e)
                {
                    Log.Error($"Exception in UpdatePlayers: {e.Message}", e);
                    throw;
                }
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
                    .ToList(),
                PlayerHand = (CardPile)GetPlayerHand(GameStage, playerId)?.Clone(),
                GameStageInfo = (GameStageInfo)MapToGameStageInfo(GameStage)?.Clone()
            };
        }

        private CardPile GetPlayerHand(GameStage gameStage, string playerId)
        {
            var playerHandDictionary = GameStage?.CurrentGameRound?.PlayerHandDictionary;
            var hand = playerHandDictionary != null && playerHandDictionary.ContainsKey(playerId)
                ? playerHandDictionary[playerId]
                : null;
            return hand;
        }

        private StickRoundInfo MapToStickRoundInfo(StickRound stickRound)
        {
            var stickRoundInfo = new StickRoundInfo
            {
                PlayActionList = stickRound?.PlayActionList,
                StickResult = stickRound?.StickResult
            };
            return stickRoundInfo;
        }

        private GameStageInfo MapToGameStageInfo(GameStage gameStage)
        {
            var gameRound = gameStage?.CurrentGameRound;
            var gameStageInfo = new GameStageInfo
            {
                CurrentBetActionList = gameRound?.BetStage?.BetActionList,
                CurrentBetResult = gameRound?.BetStage?.BetResult,
                StickRoundInfoList = gameRound?.PlayStage?.StickRoundList
                    ?.Select(r => MapToStickRoundInfo(r))
                    .ToList(),
                CurrentPlayResult = gameRound?.PlayResult,
                RoundResultList = gameStage?.GameRoundList
                    ?.Select(r => r.RoundResult)
                    .ToList(),
                GameResult = gameStage?.GameResult
            };
            return gameStageInfo;
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