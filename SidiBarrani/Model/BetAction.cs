using System;

namespace SidiBarrani.Model
{
    public class BetAction : IEquatable<BetAction>
    {
        public Player Player {get;set;}
        public BetActionType Type {get;set;}
        public Bet Bet {get;set;}

        public bool Equals(BetAction other)
        {
            if (other == null) {
                return false;
            }
            return Bet == other.Bet && Type == other.Type && Player == other.Player;
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
        public override int GetHashCode()
        {
           	unchecked
            {
                var hashCode = 13;
                hashCode = (hashCode * 397) ^ Bet.GetHashCode();
                hashCode = (hashCode * 397) ^ Type.GetHashCode();
                hashCode = (hashCode * 397) ^ Player.GetHashCode();
                return hashCode;
            }
        }
        public static bool operator ==(BetAction lhs, BetAction rhs)
        {
            if (Object.ReferenceEquals(lhs, null))
            {
                return Object.ReferenceEquals(rhs, null);
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
                return $"{Player}: {Type}";
            }
            var str = $"{Player}: {Bet}";
            return str;
        }
    }
}