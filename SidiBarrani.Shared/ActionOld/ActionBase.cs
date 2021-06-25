using SidiBarraniCommon.InfoOld;

namespace SidiBarraniCommon.ActionOld
{
    public abstract record ActionBase(GameInfo? GameInfo = null, PlayerInfo? PlayerInfo = null)
    {
        public abstract int GetActionId();
        public abstract ActionType GetActionType();

        public override string ToString()
        {
            return $"{GameInfo?.GameName} ({PlayerInfo?.PlayerName})";
        }
    }
}