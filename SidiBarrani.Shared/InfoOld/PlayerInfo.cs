namespace SidiBarraniCommon.InfoOld
{
    public record PlayerInfo(string PlayerName, string PlayerId)
    {
        public const string CurrentRelativePlayerId = "0";
        public const string NextRelativePlayerId = "1";
        public const string OppositeRelativePlayerId = "2";
        public const string PreviousRelativePlayerId = "3";

        public override string ToString() => PlayerName;
    }
}
