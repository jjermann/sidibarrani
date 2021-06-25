using System;

namespace SidiBarrani.Shared.Model.Game
{
    public static class PlayTypeExtensionMethods
    {
        public static bool IsTrump(this PlayType playType)
        {
            var isTrumpPlayType = playType == PlayType.TrumpDiamonds
                || playType == PlayType.TrumpHearts
                || playType == PlayType.TrumpClubs
                || playType == PlayType.TrumpSpades;
            return isTrumpPlayType;
        }
        public static CardSuit GetTrumpSuit(this PlayType playType)
        {
            return playType switch
            {
                PlayType.TrumpHearts => CardSuit.Hearts,
                PlayType.TrumpDiamonds => CardSuit.Diamonds,
                PlayType.TrumpClubs => CardSuit.Clubs,
                PlayType.TrumpSpades => CardSuit.Spades,
                _ => throw new NotImplementedException()
            };
        }

        public static string GetStringRepresentation(this PlayType playtype)
        {
            return playtype switch
            {
                PlayType.TrumpDiamonds => "Diamonds",
                PlayType.TrumpHearts => "Hearts",
                PlayType.TrumpClubs => "Clubs",
                PlayType.TrumpSpades => "Spades",
                PlayType.UpDown => "UpDown",
                PlayType.DownUp => "DownUp",
                _ => throw new NotImplementedException()
            };
        }
    }
}