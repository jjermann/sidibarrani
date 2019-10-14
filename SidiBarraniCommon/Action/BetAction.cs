using System;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;

namespace SidiBarraniCommon.Action
{
    public class BetAction : ActionBase, IEquatable<BetAction>
    {
        public BetActionType Type { get; set; }
        public Bet Bet { get; set; }

        public bool Equals(BetAction other)
        {
            if (other == null)
            {
                return false;
            }
            return base.Equals(other) && Bet == other.Bet && Type == other.Type;
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
            return Equals(other as BetAction);
        }
        public override ActionType GetActionType() => ActionType.BetAction;
        // Remark: ActionId doesn't depend on any temporary state.
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
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = hashCode * 397 ^ GetActionId();
                return hashCode;
            }
        }
        public static bool operator ==(BetAction lhs, BetAction rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BetAction lhs, BetAction rhs)
        {
            return !(lhs == rhs);
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

        public override object Clone()
        {
            return new BetAction
            {
                GameInfo = (GameInfo)GameInfo?.Clone(),
                PlayerInfo = (PlayerInfo)PlayerInfo?.Clone(),
                Type = Type,
                Bet = (Bet)Bet?.Clone()
            };
        }
    }
}