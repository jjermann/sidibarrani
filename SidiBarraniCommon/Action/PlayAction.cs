using System;
using SidiBarraniCommon.Model;

namespace SidiBarraniCommon.Action
{
    public class PlayAction : ActionBase, IEquatable<PlayAction>
    {
        public Card Card { get; set; }


        public bool Equals(PlayAction other)
        {
            if (other == null)
            {
                return false;
            }
            return base.Equals(other) && Card == other.Card;
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
            return Equals(other as PlayAction);
        }
        public override ActionType GetActionType() => ActionType.PlayAction;
        // Remark: ActionId doesn't depend on any temporary state.
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
        public static bool operator ==(PlayAction lhs, PlayAction rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }
        public static bool operator !=(PlayAction lhs, PlayAction rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            var str = $"{base.ToString()}: Plays {Card}";
            return str;
        }
    }
}