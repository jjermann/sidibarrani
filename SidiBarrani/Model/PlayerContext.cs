using System.Collections.Generic;
using DynamicData;

namespace SidiBarrani.Model
{
    public class PlayerContext
    {
        public ISourceList<Card> CardsInHand {get;set;}
        public ISourceList<StickPile> WonSticks {get;set;}
        public ISourceList<BetAction> AvailableBetActions {get;set;}
        public ISourceList<PlayAction> AvailablePlayActions {get;set;}
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