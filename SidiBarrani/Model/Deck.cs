using System.Collections.Generic;

namespace SidiBarrani.Model
{
    public class Deck
    {
        public IList<Card> Cards {get; set;}

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
                new Card {CardSuit = CardSuit.Diamonds, CardRank = CardRank.Six},
                new Card {CardSuit = CardSuit.Diamonds, CardRank = CardRank.Seven},
                new Card {CardSuit = CardSuit.Diamonds, CardRank = CardRank.Eight},
                new Card {CardSuit = CardSuit.Diamonds, CardRank = CardRank.Nine},
                new Card {CardSuit = CardSuit.Diamonds, CardRank = CardRank.Ten},
                new Card {CardSuit = CardSuit.Diamonds, CardRank = CardRank.Jack},
                new Card {CardSuit = CardSuit.Diamonds, CardRank = CardRank.Queen},
                new Card {CardSuit = CardSuit.Diamonds, CardRank = CardRank.King},
                new Card {CardSuit = CardSuit.Diamonds, CardRank = CardRank.Ace},
                new Card {CardSuit = CardSuit.Spades, CardRank = CardRank.Six},
                new Card {CardSuit = CardSuit.Spades, CardRank = CardRank.Seven},
                new Card {CardSuit = CardSuit.Spades, CardRank = CardRank.Eight},
                new Card {CardSuit = CardSuit.Spades, CardRank = CardRank.Nine},
                new Card {CardSuit = CardSuit.Spades, CardRank = CardRank.Ten},
                new Card {CardSuit = CardSuit.Spades, CardRank = CardRank.Jack},
                new Card {CardSuit = CardSuit.Spades, CardRank = CardRank.Queen},
                new Card {CardSuit = CardSuit.Spades, CardRank = CardRank.King},
                new Card {CardSuit = CardSuit.Spades, CardRank = CardRank.Ace},
                new Card {CardSuit = CardSuit.Clubs, CardRank = CardRank.Six},
                new Card {CardSuit = CardSuit.Clubs, CardRank = CardRank.Seven},
                new Card {CardSuit = CardSuit.Clubs, CardRank = CardRank.Eight},
                new Card {CardSuit = CardSuit.Clubs, CardRank = CardRank.Nine},
                new Card {CardSuit = CardSuit.Clubs, CardRank = CardRank.Ten},
                new Card {CardSuit = CardSuit.Clubs, CardRank = CardRank.Jack},
                new Card {CardSuit = CardSuit.Clubs, CardRank = CardRank.Queen},
                new Card {CardSuit = CardSuit.Clubs, CardRank = CardRank.King},
                new Card {CardSuit = CardSuit.Clubs, CardRank = CardRank.Ace}
            };
        }
    }
}