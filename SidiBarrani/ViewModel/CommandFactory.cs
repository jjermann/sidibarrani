using System;
using System.Reactive;
using ReactiveUI;
using SidiBarraniCommon;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Cache;
using SidiBarraniCommon.Info;

namespace SidiBarrani.ViewModel
{
    public class CommandFactory
    {
        private ISidiBarraniServerApi ServerApi {get;}
        private GameInfo GameInfo {get;}
        private PlayerInfo PlayerInfo {get;}
        private ActionCache ActionCache {get;}
        
        public CommandFactory(ISidiBarraniServerApi sidiBarraniServerApi, GameInfo gameInfo, PlayerInfo playerInfo, ActionCache actionCache)
        {
            ServerApi = sidiBarraniServerApi;
            GameInfo = gameInfo;
            PlayerInfo = playerInfo;
            ActionCache = actionCache;
        }

        public ReactiveCommand<Unit,bool> ConstructCommand(int actionId) => ReactiveCommand.Create(() => ServerApi.ProcessAction(ConstructActionInfo(actionId)));
        public ActionInfo ConstructActionInfo(int actionId) => new ActionInfo
        {
            GameId = GameInfo.GameId,
            PlayerId = PlayerInfo.PlayerId,
            ActionId = actionId
        };
        public ActionBase ConstructAction(int actionId) => ActionCache.ConstructAction(GameInfo, PlayerInfo, actionId);
    }
}