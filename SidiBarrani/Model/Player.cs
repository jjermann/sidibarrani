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

        public override string ToString() {
            return Name;
        }
    }
}