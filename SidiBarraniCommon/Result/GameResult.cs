using System;
using SidiBarraniCommon.Info;

namespace SidiBarraniCommon.Result
{
    public class GameResult
    {
        public PlayerGroupInfo PlayerGroupInfo { get; set; }
        public TeamInfo Winner { get; set; }
        public int Team1FinalScore { get; set; }
        public int Team2FinalScore { get; set; }

        public override string ToString()
        {
            var str = $"Game Winner: {Winner}" + Environment.NewLine
                + $"Final score {PlayerGroupInfo.Team1}: {Team1FinalScore}" + Environment.NewLine
                + $"Final score {PlayerGroupInfo.Team2}: {Team2FinalScore}" + Environment.NewLine;
            return str;
        }
    }
}