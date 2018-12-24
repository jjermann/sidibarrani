using System;

namespace SidiBarrani.Model
{
    public class BetAmount : IComparable<BetAmount>, IEquatable<BetAmount>
    {
        private BetAmount() { }
        public BetAmount(int amount)
        {
            Amount = amount;
        }
        public BetAmount(bool isGeneralBet = false)
        {
            Amount = isGeneralBet
                ? 500
                : 257;
            IsMatchBet = true;
            IsGeneralBet = isGeneralBet;
        }
        public int Amount {get; set;}
        public bool IsMatchBet {get;}
        public bool IsGeneralBet {get;}

        public int CompareTo(BetAmount other)
        {
            return Amount.CompareTo(other.Amount);
        }
        public bool Equals(BetAmount other)
        {
            if (other == null) {
                return false;
            }
            return Amount == other.Amount;
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
            return Equals(other as BetAmount);
        }
        public override int GetHashCode()
        {
            return Amount.GetHashCode();
        }
        public static bool operator ==(BetAmount lhs, BetAmount rhs)
        {
            if (Object.ReferenceEquals(lhs, null))
            {
                return Object.ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BetAmount lhs, BetAmount rhs)
        {
            return !(lhs == rhs);
        }
        public static bool operator <=(BetAmount lhs, BetAmount rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }
        public static bool operator >=(BetAmount lhs, BetAmount rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }
        public static bool operator <(BetAmount lhs, BetAmount rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }
        public static bool operator >(BetAmount lhs, BetAmount rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }
    }
}