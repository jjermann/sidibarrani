using System.Collections.Generic;

namespace SidiBarrani.Model
{
    public class PlayerContext
    {
        public IList<Card> CardsInHand {get;set;}
        public IList<StickPile> WonSticks {get;set;}
    }
}