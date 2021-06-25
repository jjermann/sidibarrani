using System;
using System.Collections.Generic;
using System.Linq;

namespace SidiBarraniCommon.InfoOld
{
    public static class PlayerGroupInfoExtensionMethods
    {
        public static PlayerInfo GetRelativePlayerInfo(this PlayerGroupInfo playerGroupInfo, string basePlayerId, string playerId)
        {
            var playerList = playerGroupInfo.GetPlayerList();
            var playerIdList = playerList
                .Select(p => p.PlayerId)
                .ToList();
            var basePlayerIndex = playerIdList.IndexOf(basePlayerId);
            var playerIndex = playerIdList.IndexOf(playerId);
            var relativeId = ((playerIndex - basePlayerIndex + 4) % 4).ToString();
            var relativePlayerInfo = (PlayerInfo)playerList[playerIndex].Clone();
            relativePlayerInfo.PlayerId = relativeId;
            return relativePlayerInfo;
        }

        public static TeamInfo GetRelativeTeamInfo(this PlayerGroupInfo playerGroupInfo, string basePlayerId, string teamId)
        {
            var isCurrentTeam = playerGroupInfo.GetTeamOfPlayer(basePlayerId).TeamId == teamId;
            var relativePlayerGroupInfo = playerGroupInfo.GetRelativePlayerGroupInfo(basePlayerId);
            return isCurrentTeam
                ? relativePlayerGroupInfo.Team1
                : relativePlayerGroupInfo.Team2;
        }

        public static PlayerGroupInfo GetRelativePlayerGroupInfo(this PlayerGroupInfo playerGroupInfo, string playerId)
        {
            var playerList = playerGroupInfo.GetPlayerList()
                .Select(p => playerGroupInfo.GetRelativePlayerInfo(playerId, p.PlayerId))
                .OrderBy(p => p.PlayerId)
                .ToList();
            var relativeTeam1 = (TeamInfo)playerGroupInfo.GetTeamOfPlayer(playerId).Clone();
            var relativeTeam2 = (TeamInfo)playerGroupInfo.GetOpposingTeamOfPlayer(playerId).Clone();
            relativeTeam1.TeamId = TeamInfo.CurrentRelativeTeamId;
            relativeTeam1.Player1 = playerList[0];
            relativeTeam1.Player2 = playerList[2];
            relativeTeam2.TeamId = TeamInfo.OppositeRelativeTeamId;
            relativeTeam2.Player1 = playerList[1];
            relativeTeam2.Player2 = playerList[3];

            var relativePlayerGroupInfo = new PlayerGroupInfo
            {
                Team1 = relativeTeam1,
                Team2 = relativeTeam2
            };
            return relativePlayerGroupInfo;
        }

        public static PlayerInfo GetOppositePlayer(this PlayerGroupInfo playerGroupInfo, string playerId)
        {
            var playerList = playerGroupInfo.GetPlayerList();
            var playerIdList = playerList
                .Select(p => p.PlayerId)
                .ToList();
            var previousIndex = playerIdList.IndexOf(playerId);
            var oppositeIndex = (previousIndex + 2) % 4;
            return playerList[oppositeIndex];
        }

        public static PlayerInfo GetNextPlayer(this PlayerGroupInfo playerGroupInfo, string playerId)
        {
            var playerList = playerGroupInfo.GetPlayerList();
            var playerIdList = playerList
                .Select(p => p.PlayerId)
                .ToList();
            var previousIndex = playerIdList.IndexOf(playerId);
            var nextIndex = (previousIndex + 1) % 4;
            return playerList[nextIndex];
        }

        public static PlayerInfo GetPreviousPlayer(this PlayerGroupInfo playerGroupInfo, string playerId)
        {
            var playerList = playerGroupInfo.GetPlayerList();
            var playerIdList = playerList
                .Select(p => p.PlayerId)
                .ToList();
            var previousIndex = playerIdList.IndexOf(playerId);
            var nextIndex = (previousIndex + 3) % 4;
            return playerList[nextIndex];
        }

        public static TeamInfo GetOtherTeam(this PlayerGroupInfo playerGroupInfo, string teamId)
        {
            return teamId == playerGroupInfo.Team1.TeamId
                ? playerGroupInfo.Team2
                : playerGroupInfo.Team1;
        }

        public static TeamInfo GetOpposingTeamOfPlayer(this PlayerGroupInfo playerGroupInfo, string playerId)
        {
            var team = playerGroupInfo.GetTeamOfPlayer(playerId);
            return playerGroupInfo.GetOtherTeam(team.TeamId);
        }

        public static TeamInfo GetTeamOfPlayer(this PlayerGroupInfo playerGroupInfo, string playerId)
        {
            if (playerGroupInfo.Team1.Player1?.PlayerId == playerId)
            {
                return playerGroupInfo.Team1;
            }
            if (playerGroupInfo.Team1.Player2?.PlayerId == playerId)
            {
                return playerGroupInfo.Team1;
            }
            if (playerGroupInfo.Team2.Player1?.PlayerId == playerId)
            {
                return playerGroupInfo.Team2;
            }
            if (playerGroupInfo.Team2.Player2?.PlayerId == playerId)
            {
                return playerGroupInfo.Team2;
            }
            return null;
        }

        public static PlayerInfo GetPlayerInfo(this PlayerGroupInfo playerGroupInfo, string playerId)
        {
            if (playerGroupInfo.Team1.Player1?.PlayerId == playerId)
            {
                return playerGroupInfo.Team1.Player1;
            }
            if (playerGroupInfo.Team1.Player2?.PlayerId == playerId)
            {
                return playerGroupInfo.Team1.Player2;
            }
            if (playerGroupInfo.Team2.Player1?.PlayerId == playerId)
            {
                return playerGroupInfo.Team2.Player1;
            }
            if (playerGroupInfo.Team2.Player2?.PlayerId == playerId)
            {
                return playerGroupInfo.Team2.Player2;
            }
            return null;
        }

        public static TeamInfo GetTeamInfo(this PlayerGroupInfo playerGroupInfo, string teamId)
        {
            if (playerGroupInfo.Team1.TeamId == teamId)
            {
                return playerGroupInfo.Team1;
            }
            if (playerGroupInfo.Team2.TeamId == teamId)
            {
                return playerGroupInfo.Team2;
            }
            return null;
        }

        public static IList<PlayerInfo> GetPlayerList(this PlayerGroupInfo playerGroupInfo, string firstPlayerId = null)
        {
            var playerList1 = playerGroupInfo.Team1.GetPlayerList();
            var playerList2 = playerGroupInfo.Team2.GetPlayerList();
            var playerList = new List<PlayerInfo>();
            var player1 = playerList1.FirstOrDefault();
            var player2 = playerList2.FirstOrDefault();
            var player3 = playerList1.Skip(1).FirstOrDefault();
            var player4 = playerList2.Skip(1).FirstOrDefault();
            if (player1 != null) {
                playerList.Add(player1);
            }
            if (player2 != null) {
                playerList.Add(player2);
            }
            if (player3 != null) {
                playerList.Add(player3);
            }
            if (player4 != null) {
                playerList.Add(player4);
            }
            if (firstPlayerId == null)
            {
                return playerList;
            }
            var playerIndex = playerList.FindIndex(p => p.PlayerId == firstPlayerId);
            if (playerIndex < 0)
            {
                var msg = $"Non-existing PlayerId={firstPlayerId}!";
                // Log.Error(msg);
                throw new Exception(msg);
            }
            for (var i=0; i<playerIndex; i++)
            {
                var firstPlayer = playerList.First();
                playerList.RemoveAt(0);
                playerList.Add(firstPlayer);
            }
            return playerList;
       }
    }
}