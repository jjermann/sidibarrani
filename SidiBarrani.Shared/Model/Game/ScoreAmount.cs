using System;

namespace SidiBarrani.Shared.Model.Game
{
    public record ScoreAmount: IComparable<ScoreAmount>
    {
        public int Amount { get; }
        public bool IsMatch { get; }
        public bool IsGeneral { get; }

        public ScoreAmount(int amount)
        {
            Amount = amount;
        }

        public ScoreAmount(bool isMatch = false, bool isGeneral = false)
        {
            if (!isMatch && isGeneral)
            {
                var msg = "If a General is played it also counts as a match!";
                //Log.Error(msg);
                throw new ArgumentException(msg);
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

        public int GetRoundedAmount()
        {
            var remainder = Amount % 10;
            return remainder >= 5
                ? (Amount - remainder + 10)
                : (Amount - remainder);
        }

        public int CompareTo(ScoreAmount? other)
        {
            if (ReferenceEquals(other, null))
            {
                return 1;
            }

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