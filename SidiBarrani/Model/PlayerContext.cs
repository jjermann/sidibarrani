using System.Collections.Generic;

namespace SidiBarrani.Model
{
    public class PlayerContext
    {
        public PlayerContext()
        {
            WonSticks = new List<StickPile>();
            AvailableBetActions = new List<BetAction>();
            AvailablePlayActions = new List<PlayAction>();
            CardsInHand = new List<Card>();
        }
        public IList<Card> CardsInHand {get;set;}
        public IList<StickPile> WonSticks {get;set;}
        public IList<BetAction> AvailableBetActions {get;set;}
        public IList<PlayAction> AvailablePlayActions {get;set;}
        public bool IsCurrentPlayer {get;set;}
    }
}