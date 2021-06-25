using System;

namespace SidiBarrani.Shared.Model.Setup
{
    public class TeamSetup
    {
        public static TeamSetup Construct(string teamName)
        {
            return new TeamSetup
            {
                TeamId = Guid.NewGuid(),
                TeamName = teamName
            };
        }

        public Guid TeamId { get; init; }
        public string TeamName { get; init; } = null!;
        public PlayerSetup? Player1 { get; set; }
        public PlayerSetup? Player2 { get; set; }

        public override string ToString() => TeamName;
    }
}
