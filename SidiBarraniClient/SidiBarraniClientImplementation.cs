using System;
using System.Collections.Generic;
using System.Linq;
using SidiBarraniCommon;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Cache;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;

namespace SidiBarraniClient
{
    public class SidiBarraniClientImplementation : ISidiBarraniClientApi
    {
        public ISidiBarraniServerApi SidiBarraniServerApi {get;set;}
        public IList<GameInfo> OpenGameList {get;set;}
        public GameInfo GameInfo {get;set;}
        public PlayerInfo PlayerInfo {get;set;}
        public PlayerGameInfo PlayerGameInfo {get;set;}

        private ActionCache _actionCache = new ActionCache(new Rules());

        public bool SetPlayerGameInfo(PlayerGameInfo playerGameInfo)
        {
            Console.WriteLine($"{this}: SetPlayerGameInfo({playerGameInfo})");
            PlayerGameInfo = playerGameInfo;
            return true;
        }

        public bool OpenGame(string gameName = "Game", string team1Name = "Team1", string team2Name = "Team2")
        {
            Console.WriteLine($"{this}: OpenGame({gameName}, {team1Name}, {team2Name})");
            var gameInfo = SidiBarraniServerApi?.OpenGame(gameName, team1Name, team2Name);
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
            Console.WriteLine($"{this}: RefreshOpenGames()");
            OpenGameList = SidiBarraniServerApi?.ListOpenGames();
        }

        public bool ConnectToGame(GameInfo gameInfo, string playerName)
        {
            Console.WriteLine($"{this}: ConnectToGame({gameInfo}, {playerName})");
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
            Console.WriteLine($"{this}: StartGame()");
            return SidiBarraniServerApi?.StartGame(GameInfo.GameId) ?? false;
        }

        public bool ProcessAction(ActionBase action)
        {
            Console.WriteLine($"{this}: ProcessAction({action})");
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
            Console.WriteLine($"{this}: GetValidActions()");
            var actionList = PlayerGameInfo
                ?.ValidActionList
                ?.Select(id => _actionCache.ConstructAction(GameInfo, PlayerInfo, id.ActionId))
                .ToList();
            return actionList ?? new List<ActionBase>();
        }

        public override string ToString()
        {
            return $"SidiBarraniClient (Game={GameInfo}, Player={PlayerInfo})";
        }
    }
}
