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
            Func<PlayerContext, Task<BetAction>> betActionTaskGenerator,
            Func<PlayerContext, Task<PlayAction>> playActionTaskGenerator,
            Func<Task> confirmTaskGenerator)
        {
            var player1 = PlayerFactory.CreateHumanPlayer("Human1", betActionTaskGenerator, playActionTaskGenerator, confirmTaskGenerator);
            var player2 = PlayerFactory.CreateRandomPlayer("Random2");
            var team1 = Team.CreateTeam(player1, player2, "Team1");
            var player3 = PlayerFactory.CreateRandomPlayer("Random3");
            var player4 = PlayerFactory.CreateRandomPlayer("Random4");
            var team2 = Team.CreateTeam(player3, player4, "Team2");
            var playerGroup = new PlayerGroup(team1, team2);
            return playerGroup;
        }        
    }
}