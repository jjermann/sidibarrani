using System;
using System.IO;
using SidiBarrani.Model;

namespace SidiBarrani.ViewModel
{
    public class BetRepresentation
    {
        private Bet Bet {get;}
        public PlayTypeRepresentation PlayStyleRepresentation {get;}
        public ScoreAmountRepresentation ScoreAmountRepresentation {get;}

        private BetRepresentation() { }
        public BetRepresentation(Bet bet)
        {
            Bet = bet;
            PlayStyleRepresentation = new PlayTypeRepresentation(bet.PlayType, 40);
            ScoreAmountRepresentation = new ScoreAmountRepresentation(bet.BetAmount);
        }
    }
}