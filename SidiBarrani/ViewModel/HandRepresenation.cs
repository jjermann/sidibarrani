using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Model;

namespace SidiBarrani.ViewModel
{
    public class HandRepresentation : ReactiveObject
    {
        private CardPile _cardPile;
        public CardPile CardPile
        {
            get { return _cardPile; }
            set { this.RaiseAndSetIfChanged(ref _cardPile, value); }
        }

        private IList<PlayAction> _playActionList;
        public IList<PlayAction> PlayActionList
        {
            get { return _playActionList; }
            set { this.RaiseAndSetIfChanged(ref _playActionList, value); }
        }

        private ObservableAsPropertyHelper<IList<CardRepresentation>> _cardRepresentationList;
        private IList<CardRepresentation> CardRepresentationList
        {
            get { return _cardRepresentationList.Value; }
        }

        public HandRepresentation(CardPile cardPile, IList<PlayAction> playActionList)
        {
            this.WhenAnyValue(
                x => x.CardPile,
                x => x.PlayActionList,
                (cp, al) => {
                    var cardRepresentationList = cp?.Cards
                        ?.Select(c => new CardRepresentation(c, al?.SingleOrDefault(a => a.Card == c)))
                        .ToList();
                    return cardRepresentationList;
                })
                .ToProperty(this, x => x.CardRepresentationList, out _cardRepresentationList, null);

            CardPile = cardPile;
            PlayActionList = playActionList;
        }
    }
}