using System;

namespace SidiBarrani.Model
{
    public class PlayResult
    {
        public PlayerGroup PlayerGroup {get;set;}
        public ScoreAmount Team1Score {get;set;}
        public ScoreAmount Team2Score {get;set;}
        public override string ToString()
        {
            var msg = $"Score {PlayerGroup.Team1}: {Team1Score}" + Environment.NewLine
                + $"Score {PlayerGroup.Team2}: {Team2Score}" + Environment.NewLine;
            return msg;
        }
    }
}