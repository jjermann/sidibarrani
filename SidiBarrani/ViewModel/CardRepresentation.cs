using System.IO;
using System.Windows.Input;
using ReactiveUI;
using SidiBarraniCommon.Model;

namespace SidiBarrani.ViewModel
{
    public class CardRepresentation : ReactiveObject
    {
        private Card Card {get;}
        public ICommand PlayActionCommand {get;}
        private bool IsHandCard {get;}
        public bool IsHighlighted => !IsHandCard || PlayActionCommand != null;
        public string ImageSource {get;}
        public string BorderColor {get;}
        public double BorderThickness {get;}

        public CardRepresentation(Card card)
        {
            Card = card;
            var suitName = card.CardSuit.ToString().ToLowerInvariant();
            var rankName = card.CardRank.ToString().ToLowerInvariant();
            ImageSource = Path.Combine("Assets", $"{suitName}_{rankName}.png");
            BorderColor = "Black";
            BorderThickness = 1;
        }
        public CardRepresentation(Card card, ICommand playActionCommand) : this(card)
        {
            IsHandCard = true;
            PlayActionCommand = playActionCommand;
        }
    }
}