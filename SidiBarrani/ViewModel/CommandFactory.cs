using ReactiveUI;
using SidiBarraniCommon;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Cache;
using SidiBarraniCommon.Info;

namespace SidiBarrani.ViewModel
{
    public class CommandFactory
    {
        private ISidiBarraniServerApi _serverApi;
        private GameInfo _gameInfo;
        private PlayerInfo _playerInfo;
        private ActionCache _actionCache;
        
        public CommandFactory(ISidiBarraniServerApi sidiBarraniServerApi, GameInfo gameInfo, PlayerInfo playerInfo, ActionCache actionCache)
        {
            _serverApi = sidiBarraniServerApi;
            _gameInfo = gameInfo;
            _playerInfo = playerInfo;
            _actionCache = actionCache;
        }

        public IReactiveCommand ConstructCommand(int actionId) => ReactiveCommand.Create(() => _serverApi.ProcessAction(ConstructActionInfo(actionId)));
        public ActionInfo ConstructActionInfo(int actionId) => new ActionInfo
        {
            GameId = _gameInfo.GameId,
            PlayerId = _playerInfo.PlayerId,
            ActionId = actionId
        };
        public ActionBase ConstructAction(int actionId) => _actionCache.ConstructAction(_gameInfo, _playerInfo, actionId);
    }
}