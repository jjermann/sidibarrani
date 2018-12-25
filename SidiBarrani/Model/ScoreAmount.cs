using System;

namespace SidiBarrani.Model
{
    public class ScoreAmount : IComparable<ScoreAmount>, IEquatable<ScoreAmount>
    {
        private ScoreAmount() { }
        public ScoreAmount(int amount)
        {
            Amount = amount;
        }
        public ScoreAmount(bool isMatch = false, bool isGeneral = false)
        {
            if (isMatch == false && isGeneral == true)
            {
                throw new ArgumentException();
            }
            if (isGeneral)
            {
                Amount = 500;
            }
            else if (isMatch)
            {
                Amount = 257;
            }
            else
            {
                Amount = 0;
            }
            IsMatch = isMatch;
            IsGeneral = isGeneral;
        }
        public int Amount {get; set;}
        public bool IsMatch {get;}
        public bool IsGeneral {get;}

        public int GetRoundedAmount()
        {
            var remainder = Amount % 10;
            return remainder >= 5
                ? (Amount - remainder + 10)
                : (Amount - remainder);
        }

        public int CompareTo(ScoreAmount other)
        {
            var cmp = Amount.CompareTo(other.Amount);
            if (cmp != 0 || Amount != 157)
            {
                return cmp;
            }
            if (IsMatch == other.IsMatch)
            {
                return 0;
            }
            if (IsMatch)
            {
                return 1;
            }
            return -1;
        }
        public bool Equals(ScoreAmount other)
        {
            if (other == null) {
                return false;
            }
            return Amount == other.Amount && IsMatch == other.IsMatch;
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
            return Equals(other as ScoreAmount);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 13;
                hashCode = (hashCode * 397) ^ Amount.GetHashCode();
                hashCode = (hashCode * 397) ^ IsMatch.GetHashCode();
                return hashCode;
            }
        }
        public static bool operator ==(ScoreAmount lhs, ScoreAmount rhs)
        {
            if (Object.ReferenceEquals(lhs, null))
            {
                return Object.ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }
        public static bool operator !=(ScoreAmount lhs, ScoreAmount rhs)
        {
            return !(lhs == rhs);
        }
        public static bool operator <=(ScoreAmount lhs, ScoreAmount rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }
        public static bool operator >=(ScoreAmount lhs, ScoreAmount rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }
        public static bool operator <(ScoreAmount lhs, ScoreAmount rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }
        public static bool operator >(ScoreAmount lhs, ScoreAmount rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }
        public override string ToString()
        {
            if (IsGeneral)
            {
                return $"General ({Amount})";
            }
            if (IsMatch)
            {
                return $"Match ({Amount})";
            }
            return $"{Amount}";
        }
    }
}