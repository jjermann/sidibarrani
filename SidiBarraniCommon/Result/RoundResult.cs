using System;
using SidiBarraniCommon.Info;

namespace SidiBarraniCommon.Result
{
    public class RoundResult : ICloneable
    {
        public TeamInfo WinningTeam { get; set; }
        public PlayerGroupInfo PlayerGroup { get; set; }
        public int Team1FinalScore { get; set; }
        public int Team2FinalScore { get; set; }

        public object Clone()
        {
            return new RoundResult
            {
                WinningTeam = (TeamInfo)WinningTeam?.Clone(),
                PlayerGroup = (PlayerGroupInfo)PlayerGroup?.Clone(),
                Team1FinalScore = Team1FinalScore,
                Team2FinalScore = Team2FinalScore
            };
        }

        public override string ToString()
        {
            var str = $"Round Winner: {WinningTeam}" + Environment.NewLine
                + $"Final score {PlayerGroup.Team1}: {Team1FinalScore}" + Environment.NewLine
                + $"Final score {PlayerGroup.Team2}: {Team2FinalScore}" + Environment.NewLine;
            return str;
        }
    }
}