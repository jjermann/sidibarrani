using System;

namespace SidiBarrani.Model
{
    public class Bet : IComparable<Bet>, IEquatable<Bet>
    {
        private Bet() { }
        public Bet(PlayType playType, int amount)
        {
            BetAmount = new BetAmount(amount);
            PlayType = playType;
        }
        public Bet(PlayType playType, bool isGeneralType = false)
        {
            BetAmount = new BetAmount(isGeneralType);
            PlayType = playType;
        }
        public BetAmount BetAmount {get;}
        public PlayType PlayType {get;}

        public int CompareTo(Bet other)
        {
            return BetAmount.CompareTo(other.BetAmount);
        }
        public bool Equals(Bet other)
        {
            if (other == null) {
                return false;
            }
            return BetAmount.Equals(other.BetAmount) && PlayType == other.PlayType;
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
            return Equals(other as Bet);
        }
        public override int GetHashCode()
        {
           	unchecked
            {
                var hashCode = 13;
                hashCode = (hashCode * 397) ^ BetAmount.GetHashCode();
                hashCode = (hashCode * 397) ^ PlayType.GetHashCode();
                return hashCode;
            }
        }
        public static bool operator ==(Bet lhs, Bet rhs)
        {
            if (Object.ReferenceEquals(lhs, null))
            {
                return Object.ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }
        public static bool operator !=(Bet lhs, Bet rhs)
        {
            return !(lhs == rhs);
        }
        public static bool operator <=(Bet lhs, Bet rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }
        public static bool operator >=(Bet lhs, Bet rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }
        public static bool operator <(Bet lhs, Bet rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }
        public static bool operator >(Bet lhs, Bet rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }

        public override string ToString()
        {
            var str = $"{PlayType.GetStringRepresentation()}";
            if (BetAmount.IsGeneralBet)
            {
                str += " General (500)";
            }
            else if (BetAmount.IsMatchBet)
            {
                str += " Match (257)";
            }
            else {
                str += $" {BetAmount.Amount}";
            }
            return str;
        }
    }
}