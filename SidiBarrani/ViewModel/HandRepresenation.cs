using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using SidiBarrani.Model;

namespace SidiBarrani.ViewModel
{
    public class HandRepresentation : ReactiveObject
    {
        private ObservableAsPropertyHelper<IReadOnlyCollection<CardRepresentation>> _cardsInHand;
        public IReadOnlyCollection<CardRepresentation> CardsInHand {
            get { return _cardsInHand.Value; }
        }

        private PlayerContext PlayerContext {get;}
        private HandRepresentation() { }
        public HandRepresentation(PlayerContext playerContext)
        {
            PlayerContext = playerContext;

            PlayerContext.CardsInHand
                .Connect()
                .Transform(c => new CardRepresentation(c))
                .ToCollection()
                .ToProperty(this, x => x.CardsInHand, out _cardsInHand, new ReadOnlyCollection<CardRepresentation>(new List<CardRepresentation>()));
        }
    }
}