using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using Serilog;
using SidiBarraniCommon;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Cache;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;

namespace SidiBarraniClient
{
    public class SidiBarraniClientImplementation : ReactiveObject, ISidiBarraniClientApi
    {
        private ISidiBarraniServerApi SidiBarraniServerApi {get;}

        private IList<GameInfo> _openGameList;
        public IList<GameInfo> OpenGameList {
            get { return _openGameList; }
            set { this.RaiseAndSetIfChanged(ref _openGameList, value); }
        }

        private GameInfo _gameInfo;
        public GameInfo GameInfo
        {
            get { return _gameInfo; }
            set { this.RaiseAndSetIfChanged(ref _gameInfo, value); }
        }

        private PlayerInfo _playerInfo;
        public PlayerInfo PlayerInfo
        {
            get { return _playerInfo; }
            set { this.RaiseAndSetIfChanged(ref _playerInfo, value); }
        }

        private PlayerGameInfo _playerGameInfo;
        public PlayerGameInfo PlayerGameInfo {
            get { return _playerGameInfo; }
            set { this.RaiseAndSetIfChanged(ref _playerGameInfo, value); }
        }

        private ObservableAsPropertyHelper<ActionCache> _actionCache;
        private ActionCache ActionCache
        {
            get { return _actionCache.Value; }
        }

        public SidiBarraniClientImplementation(ISidiBarraniServerApi sidiBarraniServerApi)
        {
            SidiBarraniServerApi = sidiBarraniServerApi;
            this.WhenAnyValue(x => x.GameInfo, x => x.GameInfo.Rules, (gameInfo, r) => gameInfo?.Rules)
                .Select(x => x != null ? new ActionCache(x) : null)
                .ToProperty(this, x => x.ActionCache, out _actionCache, null);
        }

        public bool RequestConfirm()
        {
            return true;
        }

        public bool SetPlayerGameInfo(PlayerGameInfo playerGameInfo)
        {
            Log.Verbose($"{this}: SetPlayerGameInfo({playerGameInfo})");
            PlayerGameInfo = playerGameInfo;
            return true;
        }

        public bool OpenGame(Rules rules = null, string gameName = "Game", string team1Name = "Team1", string team2Name = "Team2")
        {
            Log.Verbose($"{this}: OpenGame({rules}, {gameName}, {team1Name}, {team2Name})");
            var gameInfo = SidiBarraniServerApi?.OpenGame(rules, gameName, team1Name, team2Name);
            if (gameInfo == null)
            {
                return false;
            }
            GameInfo = gameInfo;
            PlayerInfo = null;
            PlayerGameInfo = null;
            RefreshOpenGames();
            return true;
        }

        public void RefreshOpenGames()
        {
            Log.Verbose($"{this}: RefreshOpenGames()");
            OpenGameList = SidiBarraniServerApi?.ListOpenGames();
        }

        public bool ConnectToGame(GameInfo gameInfo, string playerName)
        {
            Log.Verbose($"{this}: ConnectToGame({gameInfo}, {playerName})");
            var playerInfo = SidiBarraniServerApi?.ConnectToGame(gameInfo.GameId, playerName, this);
            if (playerInfo == null)
            {
                return false;
            }
            GameInfo = gameInfo;
            PlayerInfo = playerInfo;
            PlayerGameInfo = null;
            return true;
        }

        public bool StartGame()
        {
            Log.Verbose($"{this}: StartGame()");
            return SidiBarraniServerApi?.StartGame(GameInfo.GameId) ?? false;
        }

        public bool ProcessAction(ActionBase action)
        {
            Log.Verbose($"{this}: ProcessAction({action})");
            var actionInfo = new ActionInfo
            {
                GameId = GameInfo?.GameId,
                PlayerId = PlayerInfo?.PlayerId,
                ActionId = action.GetActionId()
            };
            return SidiBarraniServerApi?.ProcessAction(actionInfo) ?? false;
        }

        public IList<ActionBase> GetValidActions()
        {
            Log.Verbose($"{this}: GetValidActions()");
            var actionList = PlayerGameInfo
                ?.ValidActionList
                ?.Select(id => ActionCache.ConstructAction(GameInfo, PlayerInfo, id.ActionId))
                .ToList();
            return actionList ?? new List<ActionBase>();
        }

        public override string ToString()
        {
            return $"SidiBarraniClient (Game={GameInfo}, Player={PlayerInfo})";
        }
    }
}
