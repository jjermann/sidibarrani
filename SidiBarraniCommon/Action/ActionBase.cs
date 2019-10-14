using System;
using SidiBarraniCommon.Info;

namespace SidiBarraniCommon.Action
{
    public abstract class ActionBase : IEquatable<ActionBase>, ICloneable
    {
        public GameInfo GameInfo { get; set; }
        public PlayerInfo PlayerInfo { get; set; }


        public bool Equals(ActionBase other)
        {
            if (other == null)
            {
                return false;
            }
            return GameInfo.GameId == other.GameInfo.GameId && PlayerInfo.PlayerId == other.PlayerInfo.PlayerId;
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
            return Equals(other as ActionBase);
        }
        abstract public int GetActionId();
        abstract public ActionType GetActionType();

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 13;
                hashCode = hashCode * 397 ^ GameInfo.GameId.GetHashCode();
                hashCode = hashCode * 397 ^ PlayerInfo.PlayerId.GetHashCode();
                return hashCode;
            }
        }
        public static bool operator ==(ActionBase lhs, ActionBase rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }
        public static bool operator !=(ActionBase lhs, ActionBase rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return $"{GameInfo.GameName} ({PlayerInfo.PlayerName})";
        }

        public abstract object Clone();
    }
}