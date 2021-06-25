using System;

namespace SidiBarrani.Shared.Model.Game
{
    public record Bet(ScoreAmount BetAmount, PlayType PlayType): IComparable<Bet>
    {
        public Bet(PlayType playType, int amount)
            : this(new ScoreAmount(amount), playType)
        { }

        public Bet(PlayType playType, bool isGeneralType = false)
            : this(new ScoreAmount(isMatch: true, isGeneral: isGeneralType), playType)
        { }

        public bool IsSuccessFor(ScoreAmount amount)
        {
            return amount.CompareTo(BetAmount) >= 0;
        }

        public int CompareTo(Bet? other)
        {
            if (ReferenceEquals(other, null))
            {
                return 1;
            }

            return BetAmount.CompareTo(other.BetAmount);
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
            if (BetAmount.IsGeneral)
            {
                str += " General (500)";
            }
            else if (BetAmount.IsMatch)
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