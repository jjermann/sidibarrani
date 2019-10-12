using System;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;

namespace SidiBarraniCommon.Result
{
    public class PlayResult
    {
        public PlayerGroupInfo PlayerGroupInfo { get; set; }
        public ScoreAmount Team1Score { get; set; }
        public ScoreAmount Team2Score { get; set; }

        public override string ToString()
        {
            var msg = $"Score {PlayerGroupInfo.Team1}: {Team1Score}" + Environment.NewLine
                + $"Score {PlayerGroupInfo.Team2}: {Team2Score}" + Environment.NewLine;
            return msg;
        }
    }
}