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
        private PlayerContext PlayerContext {get;}
        private ObservableAsPropertyHelper<IReadOnlyCollection<CardRepresentation>> _cardsInHand;
        public IReadOnlyCollection<CardRepresentation> CardsInHand {
            get { return _cardsInHand.Value; }
        }
        public ReactiveCommand<CardRepresentation, PlayAction> PlayActionCommand {get;}

        private HandRepresentation() { }
        public HandRepresentation(PlayerContext playerContext)
        {
            PlayerContext = playerContext;
            PlayActionCommand = ReactiveCommand.Create<CardRepresentation, PlayAction>(r =>
            {
                var playAction = PlayerContext.AvailablePlayActions
                    .Items
                    .SingleOrDefault(a => a != null && r != null && a.Card == r.Card);
                return playAction;
            });
            PlayerContext.CardsInHand
                .Connect()
                .Transform(c => new CardRepresentation(c, PlayActionCommand))
                .ToCollection()
                .ToProperty(this, x => x.CardsInHand, out _cardsInHand, new ReadOnlyCollection<CardRepresentation>(new List<CardRepresentation>()));
        }
    }
}