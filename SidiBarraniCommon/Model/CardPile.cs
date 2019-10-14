using System;
using System.Collections.Generic;
using System.Linq;

namespace SidiBarraniCommon.Model
{
    public class CardPile : ICloneable
    {
        public IList<Card> Cards { get; set; }

        public CardPile(IList<Card> cardList = null)
        {
            var cards = new List<Card>();
            if (cardList != null)
            {
                cards.AddRange(cardList);
            }
            Cards = cards;
        }

        public void Shuffle()
        {
            Cards = Cards
                .OrderBy(c => Guid.NewGuid())
                .ToList();
        }

        public IList<Card> Draw(int n = 1)
        {
            if (Cards.Count < n)
            {
                throw new InvalidOperationException();
            }
            var drawnCards = Cards
                .Skip(Math.Max(0, Cards.Count() - n))
                .ToList();
            foreach (var card in drawnCards)
            {
                Cards.Remove(card);
            }
            var orderedCards = drawnCards
                .OrderBy(c => c.CardSuit)
                .ThenBy(c => c.CardRank)
                .ToList();
            return orderedCards;
        }

        public static CardPile CreateDeckPile()
        {
            var cards = new List<Card>
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
            var cardPile = new CardPile(cards);
            cardPile.Shuffle();
            return cardPile;
        }

        public override string ToString()
        {
            return string.Join(", ", Cards.Select(c => c.ToString()).Reverse());
        }

        public object Clone()
        {
            return new CardPile(Cards);
        }
    }
}