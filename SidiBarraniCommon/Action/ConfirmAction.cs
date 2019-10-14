using System;
using SidiBarraniCommon.Info;

namespace SidiBarraniCommon.Action
{
    public class ConfirmAction : ActionBase, IEquatable<ConfirmAction>
    {
        public bool Equals(ConfirmAction other)
        {
            if (other == null)
            {
                return false;
            }
            return base.Equals(other);
        }
        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (other.GetType() != GetType())
            {
                return false;
            }
            return Equals(other as ConfirmAction);
        }
        public override ActionType GetActionType() => ActionType.ConfirmAction;
        // Remark: ActionId doesn't depend on any temporary state.
        public override int GetActionId()
        {
            return GetStaticActionId();
        }
        public static int GetStaticActionId()
        {
            return ActionType.ConfirmAction.GetHashCode();
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
        public static bool operator ==(ConfirmAction lhs, ConfirmAction rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }
        public static bool operator !=(ConfirmAction lhs, ConfirmAction rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return $"{base.ToString()}: Confirmed";
        }

        public override object Clone()
        {
            return new ConfirmAction
            {
                GameInfo = (GameInfo)GameInfo?.Clone(),
                PlayerInfo = (PlayerInfo)PlayerInfo?.Clone(),
            };
        }
    }
}