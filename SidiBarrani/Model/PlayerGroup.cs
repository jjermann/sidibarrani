using System;
using System.Collections.Generic;
using System.Linq;

namespace SidiBarrani.Model
{
    public class PlayerGroup
    {
        private PlayerGroup() { }
        public PlayerGroup(Team team1, Team team2)
        {
            Team1 = team1;
            Team2 = team2;
        }
        public Team Team1 {get;}
        public Team Team2 {get;}

        public IList<Player> GetPlayerList()
        {
            var playerList = new List<Player>
            {   
                Team1.Player1,
                Team2.Player1,
                Team1.Player2,
                Team2.Player2
            };
            return playerList;
        }

        public IList<Player> GetPlayerListFromInitialPlayer(Player initialPlayer)
        {
            var playerList = GetPlayerList();
            while (playerList.First() != initialPlayer)
            {
                var tmpFirstPlayer = playerList.First();
                playerList.RemoveAt(0);
                playerList.Add(tmpFirstPlayer);
            }
            return playerList;
        }

        public Player GetRandomPlayer()
        {
            var randomPlayer = GetPlayerList()
                .OrderBy(p => Guid.NewGuid())
                .First();
            return randomPlayer;
        }

        public Player GetNextPlayer(Player previousPlayer)
        {
            var playerList = GetPlayerList();
            var previousIndex = playerList.IndexOf(previousPlayer);
            var nextIndex = previousIndex == 3
                ? 0
                : previousIndex + 1;
            return playerList[nextIndex];
        }

        public Team GetOtherTeam(Team team)
        {
            return team == Team1
                ? Team2
                : Team1;
        }

        public Team GetOtherTeam(Player player)
        {
            return GetOtherTeam(player.Team);
        }
    }
}