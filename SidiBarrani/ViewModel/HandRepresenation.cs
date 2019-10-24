using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;
using SidiBarraniCommon.Model;

namespace SidiBarrani.ViewModel
{
    public class HandRepresentation : ReactiveObject
    {
        private IList<Card> CardList {get;}
        private IList<CardRepresentation> CardRepresentationList {get;}
        public IDictionary<Card, ICommand> CardCommandDictionary {get;}

        public HandRepresentation(IList<Card> cardList, IDictionary<Card, ICommand> commandDictionary)
        {
            CardList = cardList;
            CardCommandDictionary = commandDictionary;
            CardRepresentationList = CardList
                ?.Select(c => new CardRepresentation(c, (CardCommandDictionary?.ContainsKey(c) ?? false) ? CardCommandDictionary[c] : null))
                .ToList();
        }
    }
}