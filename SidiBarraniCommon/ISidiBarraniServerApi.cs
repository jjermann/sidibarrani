using System.Collections.Generic;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;

namespace SidiBarraniCommon
{
    public interface ISidiBarraniServerApi
    {
        GameInfo OpenGame(Rules rules = null, string gameName = "Game", string team1Name = "Team1", string team2Name = "Team2");
        IList<GameInfo> ListOpenGames();
        // ISidiBarraniClientApi can't just be passed, it must be setup in some way
        PlayerInfo ConnectToGame(string gameId, string playerName, ISidiBarraniClientApi clientApi);
        PlayerInfo ConnectToTeam(string gameId, string teamId, string playerName, ISidiBarraniClientApi clientApi);
        bool StartGame(string gameId);
        bool ProcessAction(ActionInfo action);
    }
}
