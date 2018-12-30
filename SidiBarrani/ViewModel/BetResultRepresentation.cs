using System;
using System.IO;
using SidiBarrani.Model;

namespace SidiBarrani.ViewModel
{
    public class BetResultRepresentation
    {
        private BetResult BetResult {get;}
        public PlayTypeRepresentation PlayStyleRepresentation {get;}
        public ScoreAmountRepresentation ScoreAmountRepresentation {get;}
        public bool IsSidi {get;}
        public bool IsBarrani {get;}

        private BetResultRepresentation() { }
        public BetResultRepresentation(BetResult betResult)
        {
            BetResult = betResult;
            PlayStyleRepresentation = new PlayTypeRepresentation(betResult.Bet.PlayType, 40);
            ScoreAmountRepresentation = new ScoreAmountRepresentation(betResult.Bet.BetAmount);
            IsSidi = !betResult.IsBarrani && betResult.IsSidi;
            IsBarrani = betResult.IsBarrani;
        }
    }
}