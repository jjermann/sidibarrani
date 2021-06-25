using System;
using System.Collections.Generic;
using System.Linq;
using SidiBarraniCommon.Model;

namespace SidiBarraniServer.Game
{
    public class CardPile : ICloneable
    {
        public IList<Card> Cards { get; set; }

        public CardPile(IList<Card>? cardList = null)
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
                .OrderBy(_ => Guid.NewGuid())
                .ToList();
        }

        public IList<Card> Draw(int n = 1)
        {
            if (Cards.Count < n)
            {
                var msg = "No more cards to draw!";
                // Log.Error(msg);
                throw new InvalidOperationException(msg);
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
                new(CardSuit.Hearts, CardRank.Six),
                new(CardSuit.Hearts, CardRank.Seven),
                new(CardSuit.Hearts, CardRank.Eight),
                new(CardSuit.Hearts, CardRank.Nine),
                new(CardSuit.Hearts, CardRank.Ten),
                new(CardSuit.Hearts, CardRank.Jack),
                new(CardSuit.Hearts, CardRank.Queen),
                new(CardSuit.Hearts, CardRank.King),
                new(CardSuit.Hearts, CardRank.Ace),
                new(CardSuit.Diamonds, CardRank.Six),
                new(CardSuit.Diamonds, CardRank.Seven),
                new(CardSuit.Diamonds, CardRank.Eight),
                new(CardSuit.Diamonds, CardRank.Nine),
                new(CardSuit.Diamonds, CardRank.Ten),
                new(CardSuit.Diamonds, CardRank.Jack),
                new(CardSuit.Diamonds, CardRank.Queen),
                new(CardSuit.Diamonds, CardRank.King),
                new(CardSuit.Diamonds, CardRank.Ace),
                new(CardSuit.Spades, CardRank.Six),
                new(CardSuit.Spades, CardRank.Seven),
                new(CardSuit.Spades, CardRank.Eight),
                new(CardSuit.Spades, CardRank.Nine),
                new(CardSuit.Spades, CardRank.Ten),
                new(CardSuit.Spades, CardRank.Jack),
                new(CardSuit.Spades, CardRank.Queen),
                new(CardSuit.Spades, CardRank.King),
                new(CardSuit.Spades, CardRank.Ace),
                new(CardSuit.Clubs, CardRank.Six),
                new(CardSuit.Clubs, CardRank.Seven),
                new(CardSuit.Clubs, CardRank.Eight),
                new(CardSuit.Clubs, CardRank.Nine),
                new(CardSuit.Clubs, CardRank.Ten),
                new(CardSuit.Clubs, CardRank.Jack),
                new(CardSuit.Clubs, CardRank.Queen),
                new(CardSuit.Clubs, CardRank.King),
                new(CardSuit.Clubs, CardRank.Ace)
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