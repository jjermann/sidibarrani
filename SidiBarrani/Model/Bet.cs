namespace SidiBarrani.Model
{
    public class Bet
    {
        private Bet() { }
        public Bet(PlayType playType, int amount)
        {
            BetAmount = new BetAmount(amount);
            PlayType = playType;
        }
        public Bet(PlayType playType, bool isGeneralType = false)
        {
            BetAmount = new BetAmount(isGeneralType);
            PlayType = playType;
        }
        public BetAmount BetAmount {get;}
        public PlayType PlayType {get;}
    }
}