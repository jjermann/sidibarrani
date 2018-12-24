namespace SidiBarrani.Model
{
    public class BetResult
    {
        public Team BettingTeam {get;set;}
        public Bet Bet {get;set;}
        public bool IsSidi {get;set;}
        public bool IsBarrani {get;set;}

        public override string ToString()
        {
            var str = $"Team {BettingTeam} won with the Bet {Bet}";
            if (IsBarrani)
            {
                str += " (with Sidi + Barrani)";
            }
            else if (IsSidi)
            {
                str += " (with Sidi)";
            }
            str += ".";
            return str;
        }
    }
}