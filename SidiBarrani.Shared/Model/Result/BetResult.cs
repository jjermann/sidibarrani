using SidiBarrani.Shared.Model.Game;
using SidiBarrani.Shared.Model.Setup;

namespace SidiBarrani.Shared.Model.Result
{
    public record BetResult(TeamSetup BettingTeam, Bet Bet, bool IsSidi = false, bool IsBarrani = false)
    {
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