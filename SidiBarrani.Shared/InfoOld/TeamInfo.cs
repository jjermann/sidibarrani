namespace SidiBarraniCommon.InfoOld
{
    public record TeamInfo(string TeamName, string TeamId, PlayerInfo? Player1, PlayerInfo? Player2)
    {
        public const string CurrentRelativeTeamId = "0";
        public const string OppositeRelativeTeamId = "1";

        public override string ToString() => TeamName;
    }
}
