namespace SidiBarrani.Model
{
    public class BetAmount
    {
        private BetAmount() { }
        public BetAmount(int amount)
        {
            Amount = amount;
        }
        public BetAmount(bool isGeneralBet = false)
        {
            Amount = isGeneralBet
                ? 500
                : 257;
            IsMatchBet = true;
            IsGeneralBet = isGeneralBet;
        }
        public int Amount {get; set;}
        public bool IsMatchBet {get;set;}
        public bool IsGeneralBet {get;set;}
    }
}