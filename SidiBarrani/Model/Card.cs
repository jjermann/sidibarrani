using System;

namespace SidiBarrani.Model
{
    public class Card : IEquatable<Card>
    {
        public CardSuit CardSuit {get;set;}
        public CardRank CardRank {get;set;}

        public bool Equals(Card other)
        {
            if (other == null) {
                return false;
            }
            return CardSuit == other.CardSuit && CardRank == other.CardRank;
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
                hashCode = (hashCode * 397) ^ CardSuit.GetHashCode();
                hashCode = (hashCode * 397) ^ CardRank.GetHashCode();
                return hashCode;
            }
        }
        public static bool operator ==(Card lhs, Card rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return object.ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }
        public static bool operator !=(Card lhs, Card rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            var str = $"{CardRank.ToString()} of {CardSuit.ToString()}";
            return str;
        }
    }
}