namespace SidiBarrani.Shared.Model.Setup
{
    public class PlayerGroupSetup
    {
        public static PlayerGroupSetup ConstructDefault()
        {
            return new PlayerGroupSetup
            {
                Team1 = TeamSetup.Construct("Team1"),
                Team2 = TeamSetup.Construct("Team2")
            };
        }

        public TeamSetup Team1 { get; init; } = null!;
        public TeamSetup Team2 { get; init; } = null!;
    }
}