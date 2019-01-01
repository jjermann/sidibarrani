using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SidiBarrani.Model
{
    public static class PlayerGroupFactory
    {
        public static PlayerGroup CreatePlayerGroup(
            string player1Name = "Player1",
            string player2Name = "Player2",
            string player3Name = "Player3",
            string player4Name = "Player4",
            string team1Name = "Team1",
            string team2Name = "Team2")
        {
            var p1 = new Player {Name = player1Name};
            var p2 = new Player {Name = player2Name};
            var t1 = Team.CreateTeam(p1, p2, team1Name);
            var p3 = new Player {Name = player3Name};
            var p4 = new Player {Name = player4Name};
            var t2 = Team.CreateTeam(p3, p4, team2Name);
            var playerGroup = new PlayerGroup(t1, t2);
            return playerGroup;
        }
    }
}