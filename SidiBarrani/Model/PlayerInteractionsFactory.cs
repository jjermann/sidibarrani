using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;

namespace SidiBarrani.Model
{
    public static class PlayerInteractionsFactory
    {
        public static void AttachTaskGenerator(
            this Player player,
            Func<PlayerContext, Task<BetAction>> betActionTaskGenerator,
            Func<PlayerContext, Task<PlayAction>> playActionTaskGenerator,
            Func<Task> confirmTaskGenerator)
        {
            player.BetActionGenerator = GetFunctionFromTaskGenerator(betActionTaskGenerator);
            player.PlayActionGenerator = GetFunctionFromTaskGenerator(playActionTaskGenerator);
            player.ConfirmAction = GetActionFromTaskGenerator(confirmTaskGenerator);
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

        public static void AttachRandomGenerators(this Player player)
        {
            player.BetActionGenerator = RandomBetActionGenerator;
            player.PlayActionGenerator = RandomPlayActionGenerator;
            player.ConfirmAction = ImmediateConfirm;
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