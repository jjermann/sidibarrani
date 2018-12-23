using System;
using System.Collections.Generic;
using System.Linq;

namespace SidiBarrani.Model
{
    public class CardPile
    {
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
        public Card Draw()
        {
            if (!Cards.Any())
            {
                throw new InvalidOperationException();
            }
            var topCard = Cards.Last();
            Cards.Remove(topCard);
            return topCard;
        }
        private IList<Card> Cards {get;set;}

        public static CardPile CreateFullCardPile()
        {
            var deck = new Deck();
            var cardPile = new CardPile(deck.Cards);
            cardPile.Shuffle();
            return cardPile;
        }
    }
}