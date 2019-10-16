
using System.Collections.Generic;
using ReactiveUI;
using SidiBarraniCommon.Model;

namespace SidiBarrani.ViewModel
{
    public class StickPilesRepresentation : ReactiveObject
    {
        private IList<CardPile> _stickPileList;
        public IList<CardPile> StickPileList
        {
            get { return _stickPileList; }
            set { this.RaiseAndSetIfChanged(ref _stickPileList, value); }
        }

        public StickPilesRepresentation(IList<CardPile> stickPileList)
        {
            StickPileList = stickPileList;
        }
    }
}