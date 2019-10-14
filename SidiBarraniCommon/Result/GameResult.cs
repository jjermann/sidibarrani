using System;
using SidiBarraniCommon.Info;

namespace SidiBarraniCommon.Result
{
    public class GameResult : ICloneable
    {
        public PlayerGroupInfo PlayerGroupInfo { get; set; }
        public TeamInfo Winner { get; set; }
        public int Team1FinalScore { get; set; }
        public int Team2FinalScore { get; set; }

        public object Clone()
        {
            return new GameResult
            {
                PlayerGroupInfo = (PlayerGroupInfo)PlayerGroupInfo?.Clone(),
                Winner = (TeamInfo)Winner?.Clone(),
                Team1FinalScore = Team1FinalScore,
                Team2FinalScore = Team2FinalScore
            };
        }

        public override string ToString()
        {
            var str = $"Game Winner: {Winner}" + Environment.NewLine
                + $"Final score {PlayerGroupInfo.Team1}: {Team1FinalScore}" + Environment.NewLine
                + $"Final score {PlayerGroupInfo.Team2}: {Team2FinalScore}" + Environment.NewLine;
            return str;
        }
    }
}