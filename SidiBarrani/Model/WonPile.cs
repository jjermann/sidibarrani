using System.Collections.Generic;

namespace SidiBarrani.Model
{
    public class WonPile
    {
        public WonPile()
        {
            WonRoundPiles = new List<TurnPile>();
        }
        public IList<TurnPile> WonRoundPiles {get;set;}
    }
}