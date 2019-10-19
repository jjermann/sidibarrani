using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;
using SidiBarraniCommon.Model;

namespace SidiBarrani.ViewModel
{
    public class HandRepresentation : ReactiveObject
    {
        private CardPile CardPile {get;}
        private IList<CardRepresentation> CardRepresentationList {get;}
        public IDictionary<Card, ICommand> CardCommandDictionary {get;}

        public HandRepresentation(CardPile cardPile, IDictionary<Card, ICommand> commandDictionary)
        {
            CardPile = cardPile;
            CardCommandDictionary = commandDictionary;
            CardRepresentationList = CardPile?.Cards
                ?.Select(c => new CardRepresentation(c, (CardCommandDictionary?.ContainsKey(c) ?? false) ? CardCommandDictionary[c] : null))
                .ToList();
        }
    }
}