using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SidiBarrani.Model
{
    public static class Generators
    {
        public static BetAction RandomBetActionGenerator(PlayerContext playerContext)
        {
            if (!playerContext.AvailableBetActions.Any()) {
                return null;
            }
            var availableActions = playerContext.AvailableBetActions;
            if (playerContext.IsCurrentPlayer)
            {
                Task.Delay(1);
            }
            else
            {
                availableActions.Add(null);
            }

            var randomAction = availableActions.OrderBy(a => Guid.NewGuid()).FirstOrDefault();
            while (randomAction == null)
            {
                Task.Delay(1);
                randomAction = availableActions.OrderBy(a => Guid.NewGuid()).FirstOrDefault();
            }
            return randomAction;
        }

        public static PlayAction RandomPlayActionGenerator(PlayerContext playerContext)
        {
            if (!playerContext.AvailablePlayActions.Any()) {
                return null;
            }
            var availableActions = playerContext.AvailablePlayActions;
            if (playerContext.IsCurrentPlayer)
            {
                Task.Delay(1);
            }
            else
            {
                availableActions.Add(null);
            }
            var randomAction = availableActions.OrderBy(a => Guid.NewGuid()).FirstOrDefault();
            while (randomAction == null)
            {
                Task.Delay(1);
                randomAction = availableActions.OrderBy(a => Guid.NewGuid()).FirstOrDefault();
            }
            return randomAction;
        }

        public static PlayerGroup GetExamplePlayerGroup()
        {
            var player1 = new Player
            {
                Name = "Player1",
                BetActionGenerator = RandomBetActionGenerator,
                PlayActionGenerator = RandomPlayActionGenerator
            };
            var player2 = new Player
            {
                Name = "Player2",
                BetActionGenerator = RandomBetActionGenerator,
                PlayActionGenerator = RandomPlayActionGenerator
            };
            var team1 = new Team {
                Player1 = player1,
                Player2 = player2 
            };
            player1.Team = team1;
            player2.Team = team1;

            var player3 = new Player
            {
                Name = "Player3",
                BetActionGenerator = RandomBetActionGenerator,
                PlayActionGenerator = RandomPlayActionGenerator
            };
            var player4 = new Player
            {
                Name = "Player4",
                BetActionGenerator = RandomBetActionGenerator,
                PlayActionGenerator = RandomPlayActionGenerator
            };
            var team2 = new Team {
                Player1 = player3,
                Player2 = player4
            };
            player3.Team = team2;
            player4.Team = team2;
            var playerGroup = new PlayerGroup(team1, team2);
            return playerGroup;
        }        
    }
}