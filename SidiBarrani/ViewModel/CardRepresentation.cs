using System.IO;
using ReactiveUI;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Model;

namespace SidiBarrani.ViewModel
{
    public class CardRepresentation : ReactiveObject
    {
        private Card Card {get;}
        private PlayAction PlayAction {get;}
        public string ImageSource {get;}
        public string BorderColor {get;}
        public double BorderThickness {get;}

        public CardRepresentation(Card card, PlayAction playAction = null)
        {
            Card = card;
            PlayAction = playAction;
            var suitName = card.CardSuit.ToString().ToLowerInvariant();
            var rankName = card.CardRank.ToString().ToLowerInvariant();
            ImageSource = Path.Combine("Assets", $"{suitName}_{rankName}.png");
            BorderColor = "Black";
            BorderThickness = 1;
        }
    }
}