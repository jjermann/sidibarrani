using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace SidiBarrani.Model
{
    public static class PlayerFactory
    {
        //TODO
        public static Player CreateHumanPlayer(string name, Func<Task> taskGenerator)
        {
            var player = new Player
            {
                Name = name,
                BetActionGenerator = RandomBetActionGenerator,
                PlayActionGenerator = RandomPlayActionGenerator,
                ConfirmAction = GetConfirmActionFromTask(taskGenerator)
            };
            return player;
        }

        private static Action<PlayerContext> GetConfirmActionFromTask(Func<Task> taskGenerator)
        {
            var action = new Action<PlayerContext>(playerGroup => 
            {
                var task = taskGenerator();
                task.Wait();
            });
            return action;
        }

        public static Player CreateRandomPlayer(string name)
        {
            var player = new Player
            {
                Name = name,
                BetActionGenerator = RandomBetActionGenerator,
                PlayActionGenerator = RandomPlayActionGenerator,
                ConfirmAction = ImmediateConfirm
            };
            return player;
        }

        private static BetAction RandomBetActionGenerator(PlayerContext playerContext)
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

        private static PlayAction RandomPlayActionGenerator(PlayerContext playerContext)
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

        private static void ImmediateConfirm(PlayerContext playerContext)
        {
            return;
        }
    }
}