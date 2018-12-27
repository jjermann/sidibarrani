using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;

namespace SidiBarrani.Model
{
    public class Player : ReactiveObject
    {
        public Player()
        {
            Context = new PlayerContext();
            this.WhenAnyValue(x => x.BetActionGenerator)
                .Select(g => ReactiveCommand.CreateFromTask(() => Task.Run(() => g(Context))))
                .ToProperty(this, x => x.BetCommand, out _betCommand, null);
            this.WhenAnyValue(x => x.PlayActionGenerator)
                .Select(g => ReactiveCommand.CreateFromTask(() => Task.Run(() => g(Context))))
                .ToProperty(this, x => x.PlayCommand, out _playCommand, null);
        }

        private ObservableAsPropertyHelper<ReactiveCommand<Unit, BetAction>> _betCommand;
        public ReactiveCommand<Unit, BetAction> BetCommand
        {
            get { return _betCommand.Value; }
        }
        private ObservableAsPropertyHelper<ReactiveCommand<Unit, PlayAction>> _playCommand;
        public ReactiveCommand<Unit, PlayAction> PlayCommand
        {
            get { return _playCommand.Value; }
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

        public string Name {get;set;}
        public Team Team {get; set;}
        public PlayerContext Context {get;set;}
        public override string ToString() {
            return Name;
        }
    }
}