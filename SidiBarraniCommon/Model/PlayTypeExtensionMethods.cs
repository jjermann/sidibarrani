using System;

namespace SidiBarraniCommon.Model
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
            if (playType == PlayType.TrumpHearts)
            {
                return CardSuit.Hearts;
            }
            if (playType == PlayType.TrumpDiamonds)
            {
                return CardSuit.Diamonds;
            }
            if (playType == PlayType.TrumpClubs)
            {
                return CardSuit.Clubs;
            }
            if (playType == PlayType.TrumpSpades)
            {
                return CardSuit.Spades;
            }
            throw new NotImplementedException();
        }

        public static string GetStringRepresentation(this PlayType playtype)
        {
            switch (playtype)
            {
                case PlayType.TrumpDiamonds:
                    return "Diamonds";
                case PlayType.TrumpHearts:
                    return "Hearts";
                case PlayType.TrumpClubs:
                    return "Clubs";
                case PlayType.TrumpSpades:
                    return "Spades";
                case PlayType.UpDown:
                    return "UpDown";
                case PlayType.DownUp:
                    return "DownUp";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}