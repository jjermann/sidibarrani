using System;
using SidiBarrani.Shared.Model.Setup;

namespace SidiBarrani.Shared.Model.Result
{
    public record GameResult(PlayerGroupSetup PlayerGroupSetup, TeamSetup WinningTeam, int Team1FinalScore, int Team2FinalScore)
    {
        public override string ToString()
        {
            var str = $"Game Winner: {WinningTeam}" + Environment.NewLine
                + $"Final score {PlayerGroupSetup.Team1}: {Team1FinalScore}" + Environment.NewLine
                + $"Final score {PlayerGroupSetup.Team2}: {Team2FinalScore}" + Environment.NewLine;
            return str;
        }
    }
}