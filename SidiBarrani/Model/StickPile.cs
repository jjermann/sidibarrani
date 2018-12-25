using System.Collections.Generic;
using System.Linq;

namespace SidiBarrani.Model
{
    public class StickPile
    {
        public StickPile() {
            Cards = new List<Card>();
        }
        public IList<Card> Cards {get;set;}
        public override string ToString()
        {
            return string.Join(", ", Cards.Select(c => c.ToString()).Reverse());
        }
    }
}