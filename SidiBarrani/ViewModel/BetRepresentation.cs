using SidiBarraniCommon.Model;

namespace SidiBarrani.ViewModel
{
    public class BetRepresentation
    {
        public Bet Bet {get;}
        public PlayTypeRepresentation PlayStyleRepresentation {get;}
        public ScoreAmountRepresentation ScoreAmountRepresentation {get;}

        public BetRepresentation(Bet bet)
        {
            Bet = bet;
            PlayStyleRepresentation = new PlayTypeRepresentation(bet.PlayType, 40);
            ScoreAmountRepresentation = new ScoreAmountRepresentation(bet.BetAmount);
        }
    }
}