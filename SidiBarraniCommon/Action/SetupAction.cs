using System;

namespace SidiBarraniCommon.Action
{
    public class SetupAction : ActionBase, IEquatable<SetupAction>
    {
        public bool Setup { get; set; }

        public bool Equals(SetupAction other)
        {
            if (other == null)
            {
                return false;
            }
            return base.Equals(other) && Setup == other.Setup;
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
            return Equals(other as SetupAction);
        }
        public override ActionType GetActionType() => ActionType.SetupAction;
        // Remark: ActionId doesn't depend on any temporary state.
        public override int GetActionId()
        {
            unchecked
            {
                var hashCode = ActionType.SetupAction.GetHashCode();
                hashCode = hashCode * 397 ^ Setup.GetHashCode();
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
        public static bool operator ==(SetupAction lhs, SetupAction rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }
        public static bool operator !=(SetupAction lhs, SetupAction rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            var setupStr = Setup
                ? "Setup"
                : "Didn't setup";
            return $"{base.ToString()}: {setupStr}";
        }
    }
}