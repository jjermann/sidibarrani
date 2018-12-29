using System;
using System.Collections.Generic;
using System.Linq;

namespace SidiBarrani.Model
{
    public class CardPile
    {
        private IList<Card> Cards {get;set;}

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
        public IList<Card> Draw(int n=1)
        {
            if (Cards.Count < n)
            {
                throw new InvalidOperationException();
            }
            var drawnCards = Cards.TakeLast(n).ToList();
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
        public static CardPile CreateFullCardPile()
        {
            var deck = new Deck();
            var cardPile = new CardPile(deck.Cards);
            cardPile.Shuffle();
            return cardPile;
        }
        public override string ToString()
        {
            return string.Join(", ", Cards.Select(c => c.ToString()).Reverse());
        }
    }
}