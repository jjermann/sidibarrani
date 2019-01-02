using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace SidiBarrani.Model
{
    public static class PlayerInteractionsFactory
    {
        public static void AttachObservable(
            this Player player,
            IObservable<BetAction> betActionObservable,
            IObservable<PlayAction> playActionObservable,
            IObservable<Unit> confirmObservable)
        {
            player.RequestBetCommand = ReactiveCommand.CreateFromObservable(() => betActionObservable.FirstAsync());
            player.RequestPlayCommand = ReactiveCommand.CreateFromObservable(() => playActionObservable.FirstAsync());
            player.RequestConfirmCommand = ReactiveCommand.CreateFromObservable(() =>
            {
                return confirmObservable.FirstAsync();
            });
        }

        public static void AttachTaskGenerator(
            this Player player,
            Func<PlayerContext, Task<BetAction>> betActionTaskGenerator,
            Func<PlayerContext, Task<PlayAction>> playActionTaskGenerator,
            Func<PlayerContext, Task> confirmTaskGenerator)
        {
            player.RequestBetCommand = ReactiveCommand.CreateFromTask(() => betActionTaskGenerator(player.Context));
            player.RequestPlayCommand = ReactiveCommand.CreateFromTask(() => playActionTaskGenerator(player.Context));
            player.RequestConfirmCommand = ReactiveCommand.CreateFromTask(() => confirmTaskGenerator(player.Context));
        }

        public static void AttachRandomGenerators(this Player player)
        {
            player.RequestBetCommand = ReactiveCommand.Create(() => RandomBetActionGenerator(player.Context));
            player.RequestPlayCommand = ReactiveCommand.Create(() => RandomPlayActionGenerator(player.Context));
            player.RequestConfirmCommand = ReactiveCommand.Create(() => ImmediateConfirm(player.Context));
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