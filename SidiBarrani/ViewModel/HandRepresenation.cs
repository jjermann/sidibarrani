using System;
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
        private ObservableAsPropertyHelper<IReadOnlyCollection<PlayAction>> _availablePlayActionList;
        private IReadOnlyCollection<PlayAction> AvailablePlayActionList {
            get { return _availablePlayActionList.Value; }
        }
        public ReactiveCommand<CardRepresentation, PlayAction> PlayActionCommand {get;}

        private HandRepresentation() { }
        public HandRepresentation(PlayerContext playerContext)
        {
            PlayerContext = playerContext;
            PlayerContext.AvailablePlayActions
                .Connect()
                .ToCollection()
                .ToProperty(this, x => x.AvailablePlayActionList, out _availablePlayActionList, new ReadOnlyCollection<PlayAction>(new List<PlayAction>()));
            PlayActionCommand = ReactiveCommand.Create<CardRepresentation, PlayAction>(r =>
            {
                var playAction = AvailablePlayActionList
                    .SingleOrDefault(a => a != null && r != null && a.Card == r.Card);
                return playAction;
            });
            var getPlayActionObservable = new Func<Card,IObservable<PlayAction>>(c =>
                this.WhenAnyValue(x => x.AvailablePlayActionList).Select(l => l.SingleOrDefault(a => a != null && a.Card == c)));
            PlayerContext.CardsInHand
                .Connect()
                .Transform(card => new CardRepresentation(card, getPlayActionObservable(card), PlayActionCommand))
                .ToCollection()
                .ToProperty(this, x => x.CardsInHand, out _cardsInHand, new ReadOnlyCollection<CardRepresentation>(new List<CardRepresentation>()));
        }
    }
}