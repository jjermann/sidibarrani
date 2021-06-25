using SidiBarraniCommon.Model;

namespace SidiBarraniCommon.InfoOld
{
    public record GameInfo(string GameName, string GameId, Rules Rules, PlayerGroupInfo PlayerGroupInfo)
    {
        public override string ToString() => GameName;
    }
}
