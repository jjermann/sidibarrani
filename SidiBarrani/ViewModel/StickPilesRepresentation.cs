
using System.Collections.Generic;
using ReactiveUI;
using SidiBarraniCommon.Model;

namespace SidiBarrani.ViewModel
{
    public class StickPilesRepresentation : ReactiveObject
    {
        private IList<CardPile> StickPileList {get;}

        public StickPilesRepresentation(IList<CardPile> stickPileList)
        {
            StickPileList = stickPileList;
        }
    }
}