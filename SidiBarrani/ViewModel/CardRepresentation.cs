using System.IO;
using System.Reactive.Linq;
using ReactiveUI;
using SidiBarrani.Model;

namespace SidiBarrani.ViewModel
{
    public class CardRepresentation : ReactiveObject
    {
        public Card Card {get;}
        private string CardImageSource {get;}
        private string BackImageSource {get;}

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
        private bool _isFaceUp;
        public bool IsFaceUp
        {
            get {return _isFaceUp;}
            set {this.RaiseAndSetIfChanged(ref _isFaceUp, value);}
        }

        private ObservableAsPropertyHelper<string> _imageSource;
        public string ImageSource
        {
            get {return _imageSource.Value;}
        }

        public ReactiveCommand<CardRepresentation, PlayAction> PlayActionCommand {get;}

        private CardRepresentation() {}
        public CardRepresentation(Card card, ReactiveCommand<CardRepresentation, PlayAction> playActionCommand = null)
        {
            Card = card;
            var suitName = card.CardSuit.ToString().ToLowerInvariant();
            var rankName = card.CardRank.ToString().ToLowerInvariant();
            CardImageSource = Path.Combine(@"Images", $"{suitName}_{rankName}.png");
            BackImageSource = Path.Combine(@"Images", "card_back.png");

            IsFaceUp = true;
            BorderColor = "Black";
            BorderThickness = 1;

            PlayActionCommand = playActionCommand;
            this.WhenAnyValue(x => x.IsFaceUp)
                .Select(isFaceUp => isFaceUp
                    ? CardImageSource
                    : BackImageSource)
                .ToProperty(this, x => x.ImageSource, out _imageSource, null);
        }
    }
}