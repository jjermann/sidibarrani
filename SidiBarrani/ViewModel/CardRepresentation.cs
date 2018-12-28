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

        private bool _isHighlighted;
        public bool IsHighlighted
        {
            get {return _isHighlighted;}
            set {this.RaiseAndSetIfChanged(ref _isHighlighted, value);}
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

        private CardRepresentation() {}
        public CardRepresentation(Card card)
        {
            Card = card;
            var suitName = card.CardSuit.ToString().ToLowerInvariant();
            var rankName = card.CardRank.ToString().ToLowerInvariant();
            CardImageSource = Path.Combine(@"Images", $"{suitName}_{rankName}.png");
            BackImageSource = Path.Combine(@"Images", "card_back.png");
            IsFaceUp = true;

            this.WhenAnyValue(x => x.IsFaceUp)
                .Select(isFaceUp => isFaceUp
                    ? CardImageSource
                    : BackImageSource)
                .ToProperty(this, x => x.ImageSource, out _imageSource, null);
        }
    }
}