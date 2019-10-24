using System;

namespace SidiBarraniCommon.Info
{
    public class PlayerInfo : IEquatable<PlayerInfo>, ICloneable
    {
        public const string CurrentRelativePlayerId = "0";
        public const string NextRelativePlayerId = "1";
        public const string OppositeRelativePlayerId = "2";
        public const string PreviousRelativePlayerId = "3";

        public string PlayerName { get; set; }
        public string PlayerId { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public bool Equals(PlayerInfo other)
        {
            if (other == null)
            {
                return false;
            }
            return PlayerId == other.PlayerId;
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
            return Equals(other as PlayerInfo);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = PlayerId == null ? 0 : PlayerId.GetHashCode();
                return hashCode;
            }
        }
        public static bool operator ==(PlayerInfo lhs, PlayerInfo rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }
        public static bool operator !=(PlayerInfo lhs, PlayerInfo rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString() => PlayerName;
    }
}
