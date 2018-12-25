using System;

namespace SidiBarrani.Model
{
    public class PlayAction : IEquatable<PlayAction>
    {
        public Player Player {get;set;}
        public Card Card {get;set;}

        public bool Equals(PlayAction other)
        {
            if (other == null) {
                return false;
            }
            return Player == other.Player && Card == other.Card;
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
                hashCode = (hashCode * 397) ^ Card.GetHashCode();
                hashCode = (hashCode * 397) ^ Player.GetHashCode();
                return hashCode;
            }
        }
        public static bool operator ==(PlayAction lhs, PlayAction rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return object.ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }
        public static bool operator !=(PlayAction lhs, PlayAction rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            var str = $"{Player} plays {Card}";
            return str;
        }
    }
}