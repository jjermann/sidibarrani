using System;
using SidiBarrani.Shared.Model.Game;
using SidiBarrani.Shared.Model.Setup;

namespace SidiBarrani.Shared.Model.Result
{
    public record PlayResult(PlayerGroupSetup PlayerGroupSetup, ScoreAmount Team1Score, ScoreAmount Team2Score)
    {
        public override string ToString()
        {
            var msg = $"Score {PlayerGroupSetup.Team1}: {Team1Score}" + Environment.NewLine
                + $"Score {PlayerGroupSetup.Team2}: {Team2Score}" + Environment.NewLine;
            return msg;
        }
    }
}