using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Model;

namespace SidiBarrani.ViewModel
{
    public class HandRepresentation : ReactiveObject
    {
        public CardPile CardPile {get;}
        public IList<PlayAction> PlayActionList {get;}
        public IList<CardRepresentation> CardRepresentationList {get;}

        public HandRepresentation(CardPile cardPile, IList<PlayAction> playActionList)
        {
            CardPile = cardPile;
            PlayActionList = playActionList;
            CardRepresentationList = CardPile?.Cards
                ?.Select(c => new CardRepresentation(c, PlayActionList?.SingleOrDefault(a => a.Card == c)))
                .ToList();
        }
    }
}