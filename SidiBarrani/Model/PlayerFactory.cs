using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;

namespace SidiBarrani.Model
{
    public static class PlayerFactory
    {
        public static Player CreateHumanPlayer(
            string name,
            Func<PlayerContext, Task<BetAction>> betActionTaskGenerator,
            Func<PlayerContext, Task<PlayAction>> playActionTaskGenerator,
            Func<Task> confirmTaskGenerator)
        {
            var player = new Player
            {
                Name = name,
                BetActionGenerator = GetFunctionFromTaskGenerator(betActionTaskGenerator),
                PlayActionGenerator = GetFunctionFromTaskGenerator(playActionTaskGenerator),
                ConfirmAction = GetActionFromTaskGenerator(confirmTaskGenerator)
            };
            return player;
        }

        private static Func<PlayerContext, T> GetFunctionFromTaskGenerator<T>(Func<PlayerContext, Task<T>> taskGenerator)
        {
            var fun = new Func<PlayerContext, T>(playerGroup =>
            {
                var task = taskGenerator(playerGroup);
                var result = task.Result;
                return result;
            });
            return fun;
        }

        private static Action<PlayerContext> GetActionFromTaskGenerator(Func<Task> taskGenerator)
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

        public static BetAction RandomBetActionGenerator(PlayerContext playerContext)
        {
            if (!playerContext.AvailableBetActions.Items.Any()) {
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

            var randomAction = availableActions.Items.OrderBy(a => Guid.NewGuid()).FirstOrDefault();
            while (randomAction == null)
            {
                Task.Delay(1);
                randomAction = availableActions.Items.OrderBy(a => Guid.NewGuid()).FirstOrDefault();
            }
            return randomAction;
        }

        public static PlayAction RandomPlayActionGenerator(PlayerContext playerContext)
        {
            if (!playerContext.AvailablePlayActions.Items.Any()) {
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
            var randomAction = availableActions.Items.OrderBy(a => Guid.NewGuid()).FirstOrDefault();
            while (randomAction == null)
            {
                Task.Delay(1);
                randomAction = availableActions.Items.OrderBy(a => Guid.NewGuid()).FirstOrDefault();
            }
            return randomAction;
        }

        private static void ImmediateConfirm(PlayerContext playerContext)
        {
            return;
        }
    }
}