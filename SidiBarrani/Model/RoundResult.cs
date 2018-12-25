using System;

namespace SidiBarrani.Model
{
    public class RoundResult
    {
        public Team WinningTeam {get;set;}
        public PlayerGroup PlayerGroup {get;set;}
        public int Team1FinalScore {get; set;}
        public int Team2FinalScore {get; set;}
        public override string ToString()
        {
            var str = $"Round Winner: {WinningTeam}" + Environment.NewLine
                + $"Final score {PlayerGroup.Team1}: {Team1FinalScore}" + Environment.NewLine
                + $"Final score {PlayerGroup.Team2}: {Team2FinalScore}" + Environment.NewLine;
            return str;
        }
    }
}