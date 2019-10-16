using System.IO;
using ReactiveUI;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Model;

namespace SidiBarrani.ViewModel
{
    public class CardRepresentation : ReactiveObject
    {
        public Card Card {get;}
        public PlayAction PlayAction {get;}

        private string _imageSource;
        public string ImageSource
        {
            get {return _imageSource;}
            set {this.RaiseAndSetIfChanged(ref _imageSource, value);}
        }
        private string _borderColor;
        public string BorderColor
        {
            get {return _borderColor;}
            set {this.RaiseAndSetIfChanged(ref _borderColor, value);}
        }
        private double _borderThickness;
        public double BorderThickness
        {
            get {return _borderThickness;}
            set {this.RaiseAndSetIfChanged(ref _borderThickness, value);}
        }
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