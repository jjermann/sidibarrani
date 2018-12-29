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

        private ObservableAsPropertyHelper<ReactiveCommand<Unit, BetAction>> _requestBetCommand;
        public ReactiveCommand<Unit, BetAction> RequestBetCommand
        {
            get { return _requestBetCommand.Value; }
        }
        private ObservableAsPropertyHelper<ReactiveCommand<Unit, PlayAction>> _requestPlayCommand;
        public ReactiveCommand<Unit, PlayAction> RequestPlayCommand
        {
            get { return _requestPlayCommand.Value; }
        }
        private ObservableAsPropertyHelper<ReactiveCommand<Unit, Unit>> _requestConfirmCommand;
        public ReactiveCommand<Unit, Unit> RequestConfirmCommand
        {
            get { return _requestConfirmCommand.Value; }
        }
        private Func<PlayerContext, BetAction> _betActionGenerator;
        public Func<PlayerContext, BetAction> BetActionGenerator
        {
            get { return _betActionGenerator; }
            set { this.RaiseAndSetIfChanged(ref _betActionGenerator, value); }
        }
        private Func<PlayerContext, PlayAction> _playActionGenerator;
        public Func<PlayerContext, PlayAction> PlayActionGenerator
        {
            get { return _playActionGenerator; }
            set { this.RaiseAndSetIfChanged(ref _playActionGenerator, value); }
        }
        private Action<PlayerContext> _confirmAction;
        public Action<PlayerContext> ConfirmAction
        {
            get { return _confirmAction; }
            set { this.RaiseAndSetIfChanged(ref _confirmAction, value); }
        }

        public Player()
        {
            Context = new PlayerContext();
            this.WhenAnyValue(x => x.BetActionGenerator)
                .Select(g => ReactiveCommand.CreateFromTask(() => Task.Run(() => g(Context))))
                .ToProperty(this, x => x.RequestBetCommand, out _requestBetCommand, null);
            this.WhenAnyValue(x => x.PlayActionGenerator)
                .Select(g => ReactiveCommand.CreateFromTask(() => Task.Run(() => g(Context))))
                .ToProperty(this, x => x.RequestPlayCommand, out _requestPlayCommand, null);
            this.WhenAnyValue(x => x.ConfirmAction)
                .Select(g => ReactiveCommand.CreateFromTask(() => Task.Run(() => g(Context))))
                .ToProperty(this, x => x.RequestConfirmCommand, out _requestConfirmCommand, null);
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
            var confirmTask = Observable.Merge(playerList.Select(p => p.RequestConfirmCommand.Execute()))
                .LastAsync()
                .ToTask();
            await confirmTask;
        }

        public override string ToString() {
            return Name;
        }
    }
}