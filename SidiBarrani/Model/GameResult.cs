using System;

namespace SidiBarrani.Model
{
    public class GameResult
    {
        public Team Winner {get; set;}
        public PlayerGroup PlayerGroup {get; set;}
        public int Team1FinalScore {get;set;}
        public int Team2FinalScore {get;set;}
        public override string ToString()
        {
            var str = $"Game Winner: {Winner}" + Environment.NewLine
                + $"Final score {PlayerGroup.Team1}: {Team1FinalScore}" + Environment.NewLine
                + $"Final score {PlayerGroup.Team2}: {Team2FinalScore}" + Environment.NewLine;
            return str;
        }
    }
}