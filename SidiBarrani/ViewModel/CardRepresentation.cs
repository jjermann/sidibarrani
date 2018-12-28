using System.IO;
using SidiBarrani.Model;

namespace SidiBarrani.ViewModel
{
    public class CardRepresentation
    {
        public Card Card {get;}
        public string ImageName {get;}
        public CardRepresentation(Card card)
        {
            Card = card;
            var suitName = card.CardSuit.ToString().ToLowerInvariant();
            var rankName = card.CardRank.ToString().ToLowerInvariant();
            ImageName = Path.Combine(@"Images", $"{suitName}_{rankName}.png");
        }
    }
}