using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;

namespace SidiBarrani.Model
{
    public class Player : ReactiveObject
    {
        public string Name {get;set;}
        public Team Team {get; set;}
        public PlayerContext Context {get;set;}

        private ReactiveCommand<Unit, BetAction> _requestBetCommand;
        public ReactiveCommand<Unit, BetAction> RequestBetCommand
        {
            get { return _requestBetCommand; }
            set { this.RaiseAndSetIfChanged(ref _requestBetCommand, value); }
        }
        private ReactiveCommand<Unit, PlayAction> _requestPlayCommand;
        public ReactiveCommand<Unit, PlayAction> RequestPlayCommand
        {
            get { return _requestPlayCommand; }
            set { this.RaiseAndSetIfChanged(ref _requestPlayCommand, value); }
        }
        private ReactiveCommand<Unit, Unit> _requestConfirmCommand;
        public ReactiveCommand<Unit, Unit> RequestConfirmCommand
        {
            get { return _requestConfirmCommand; }
            set { this.RaiseAndSetIfChanged(ref _requestConfirmCommand, value); }
        }

        public Player()
        {
            Context = new PlayerContext();
        }

        public static async Task<BetAction> GetNextBetAction(IList<Player> playerList)
        {
            var betActionTask = Observable.Merge(playerList.Select(p => p.RequestBetCommand.Execute()))
                .FirstAsync(a => a != null)
                .ToTask();
            var betAction = await betActionTask;
            return betAction;
        }

        public static async Task<PlayAction> GetNextPlayAction(IList<Player> playerList)
        {
            var playActionTask = Observable.Merge(playerList.Select(p => p.RequestPlayCommand.Execute()))
                .FirstAsync(a => a != null)
                .ToTask();
            var playAction = await playActionTask;
            return playAction;
        }

        // TODO: Allow the possibility for players to declare victory! If they are wrong they lose.
        public static async Task GetPlayerConfirm(IList<Player> playerList)
        {
            var task = Observable.Merge(playerList.Select(p => p.RequestConfirmCommand.Execute().FirstAsync()))
                .LastAsync()
                .ToTask();
            await task;
        }

        public override string ToString() {
            return Name;
        }
    }
}