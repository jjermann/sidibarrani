
using System.Collections.Generic;
using ReactiveUI;
using SidiBarraniCommon.Model;

namespace SidiBarrani.ViewModel
{
    public class StickPilesRepresentation : ReactiveObject
    {
        private IList<IList<Card>> StickPileList {get;}

        public StickPilesRepresentation(IList<IList<Card>> stickPileList)
        {
            StickPileList = stickPileList;
        }
    }
}