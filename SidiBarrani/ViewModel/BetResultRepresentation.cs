using System;
using System.IO;
using SidiBarrani.Model;

namespace SidiBarrani.ViewModel
{
    public class BetResultRepresentation
    {
        private BetResult BetResult {get;}
        public BetRepresentation BetRepresentation {get;}
        public bool IsSidi {get;}
        public bool IsBarrani {get;}

        private BetResultRepresentation() { }
        public BetResultRepresentation(BetResult betResult)
        {
            BetResult = betResult;
            BetRepresentation = new BetRepresentation(betResult.Bet);
            IsSidi = !betResult.IsBarrani && betResult.IsSidi;
            IsBarrani = betResult.IsBarrani;
        }
    }
}