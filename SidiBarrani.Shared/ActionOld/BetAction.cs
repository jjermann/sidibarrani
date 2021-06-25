using SidiBarraniCommon.InfoOld;
using SidiBarraniCommon.Model;

namespace SidiBarraniCommon.ActionOld
{
    public record BetAction(BetActionType Type, Bet? Bet = null, GameInfo? GameInfo = null, PlayerInfo? PlayerInfo = null)
        : ActionBase(GameInfo, PlayerInfo)
    {
        public override ActionType GetActionType() => ActionType.BetAction;

        public override int GetActionId()
        {
            unchecked
            {
                var hashCode = ActionType.BetAction.GetHashCode();
                hashCode = hashCode * 397 ^ (Bet != null ? Bet.GetHashCode() : 0);
                hashCode = hashCode * 397 ^ Type.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            if (Type != BetActionType.Bet)
            {
                return $"{base.ToString()}: {Type}";
            }
            var str = $"{base.ToString()}: {Bet}";
            return str;
        }
    }
}