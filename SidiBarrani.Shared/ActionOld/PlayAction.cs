using SidiBarraniCommon.InfoOld;
using SidiBarraniCommon.Model;

namespace SidiBarraniCommon.ActionOld
{
    public record PlayAction(Card Card, GameInfo? GameInfo = null, PlayerInfo? PlayerInfo = null)
        : ActionBase(GameInfo, PlayerInfo)
    {
        public override ActionType GetActionType() => ActionType.PlayAction;

        public override int GetActionId()
        {
            unchecked
            {
                var hashCode = ActionType.PlayAction.GetHashCode();
                hashCode = hashCode * 397 ^ Card.GetHashCode();
                return hashCode;
            }
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = hashCode * 397 ^ GetActionId();
                return hashCode;
            }
        }

        public override string ToString()
        {
            var str = $"{base.ToString()}: Plays {Card}";
            return str;
        }
    }
}