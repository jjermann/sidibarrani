using System.Collections.Generic;

namespace SidiBarrani.Model
{
    public class Deck
    {
        public Deck()
        {
            Cards = new List<Card>
            {
                new Card {CardSuit = CardSuit.Hearts, CardRank = CardRank.Six},
                new Card {CardSuit = CardSuit.Hearts, CardRank = CardRank.Seven},
                new Card {CardSuit = CardSuit.Hearts, CardRank = CardRank.Eight},
                new Card {CardSuit = CardSuit.Hearts, CardRank = CardRank.Nine},
                new Card {CardSuit = CardSuit.Hearts, CardRank = CardRank.Ten},
                new Card {CardSuit = CardSuit.Hearts, CardRank = CardRank.Jack},
                new Card {CardSuit = CardSuit.Hearts, CardRank = CardRank.Queen},
                new Card {CardSuit = CardSuit.Hearts, CardRank = CardRank.King},
                new Card {CardSuit = CardSuit.Hearts, CardRank = CardRank.Ace},
                new Card {CardSuit = CardSuit.Clovers, CardRank = CardRank.Six},
                new Card {CardSuit = CardSuit.Clovers, CardRank = CardRank.Seven},
                new Card {CardSuit = CardSuit.Clovers, CardRank = CardRank.Eight},
                new Card {CardSuit = CardSuit.Clovers, CardRank = CardRank.Nine},
                new Card {CardSuit = CardSuit.Clovers, CardRank = CardRank.Ten},
                new Card {CardSuit = CardSuit.Clovers, CardRank = CardRank.Jack},
                new Card {CardSuit = CardSuit.Clovers, CardRank = CardRank.Queen},
                new Card {CardSuit = CardSuit.Clovers, CardRank = CardRank.King},
                new Card {CardSuit = CardSuit.Clovers, CardRank = CardRank.Ace},
                new Card {CardSuit = CardSuit.Tiles, CardRank = CardRank.Six},
                new Card {CardSuit = CardSuit.Tiles, CardRank = CardRank.Seven},
                new Card {CardSuit = CardSuit.Tiles, CardRank = CardRank.Eight},
                new Card {CardSuit = CardSuit.Tiles, CardRank = CardRank.Nine},
                new Card {CardSuit = CardSuit.Tiles, CardRank = CardRank.Ten},
                new Card {CardSuit = CardSuit.Tiles, CardRank = CardRank.Jack},
                new Card {CardSuit = CardSuit.Tiles, CardRank = CardRank.Queen},
                new Card {CardSuit = CardSuit.Tiles, CardRank = CardRank.King},
                new Card {CardSuit = CardSuit.Tiles, CardRank = CardRank.Ace},
                new Card {CardSuit = CardSuit.Pikes, CardRank = CardRank.Six},
                new Card {CardSuit = CardSuit.Pikes, CardRank = CardRank.Seven},
                new Card {CardSuit = CardSuit.Pikes, CardRank = CardRank.Eight},
                new Card {CardSuit = CardSuit.Pikes, CardRank = CardRank.Nine},
                new Card {CardSuit = CardSuit.Pikes, CardRank = CardRank.Ten},
                new Card {CardSuit = CardSuit.Pikes, CardRank = CardRank.Jack},
                new Card {CardSuit = CardSuit.Pikes, CardRank = CardRank.Queen},
                new Card {CardSuit = CardSuit.Pikes, CardRank = CardRank.King},
                new Card {CardSuit = CardSuit.Pikes, CardRank = CardRank.Ace}
            };
        }
        public IList<Card> Cards {get; set;}
    }
}