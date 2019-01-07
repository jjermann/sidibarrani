using System.Collections.Generic;
using DynamicData;
using ReactiveUI;

namespace SidiBarrani.Model
{
    public class PlayerContext : ReactiveObject
    {
        public ISourceList<Card> CardsInHand {get;set;}
        public ISourceList<StickPile> WonSticks {get;set;}
        public ISourceList<BetAction> AvailableBetActions {get;set;}
        public ISourceList<PlayAction> AvailablePlayActions {get;set;}
        private bool _canConfirm;
        public bool CanConfirm
        {
            get { return _canConfirm; }
            set { this.RaiseAndSetIfChanged(ref _canConfirm, value); }
        }
        public bool IsCurrentPlayer {get;set;}

        public PlayerContext()
        {
            WonSticks = new SourceList<StickPile>();
            AvailableBetActions = new SourceList<BetAction>();
            AvailablePlayActions = new SourceList<PlayAction>();
            CardsInHand = new SourceList<Card>();
        }
    }
}