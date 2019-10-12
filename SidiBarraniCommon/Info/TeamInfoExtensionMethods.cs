using System;
using System.Collections.Generic;
using System.Linq;

namespace SidiBarraniCommon.Info
{
    public static class TeamInfoExtensionMethods
    {
        public static PlayerInfo GetPlayerInfo(this TeamInfo teamInfo, string playerId)
        {
            if (teamInfo.Player1.PlayerId == playerId)
            {
                return teamInfo.Player1;
            }
            if (teamInfo.Player2.PlayerId == playerId)
            {
                return teamInfo.Player2;
            }
            return null;
        }
        public static IList<PlayerInfo> GetPlayerList(this TeamInfo teamInfo)
        {
            var playerList = new List<PlayerInfo>();
            var player1 = teamInfo.Player1;
            var player2 = teamInfo.Player2;
            if (player1 != null)
            {
                playerList.Add(player1);
            }
            if (player2 != null)
            {
                playerList.Add(player2);
            }
            return playerList;
       }
    }
}