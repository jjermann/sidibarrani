using System;

namespace SidiBarrani.Model
{
    public static class PlayTypeExtensionMethods
    {
        public static bool IsTrump(this PlayType playType)
        {
            var isTrumpPlayType = playType == PlayType.TrumpClovers
                || playType == PlayType.TrumpHearts
                || playType == PlayType.TrumpPikes
                || playType == PlayType.TrumpTiles;
            return isTrumpPlayType;
        }
        public static CardSuit GetTrumpSuit(this PlayType playType)
        {
            if (playType == PlayType.TrumpHearts)
            {
                return CardSuit.Hearts;
            }
            if (playType == PlayType.TrumpClovers)
            {
                return CardSuit.Clovers;
            }
            if (playType == PlayType.TrumpPikes)
            {
                return CardSuit.Pikes;
            }
            if (playType == PlayType.TrumpTiles)
            {
                return CardSuit.Tiles;
            }
            throw new ArgumentException();
        }

        public static string GetStringRepresentation(this PlayType playtype)
        {
            switch (playtype)
            {
                case PlayType.TrumpClovers:
                    return "Clovers";
                case PlayType.TrumpHearts:
                    return "Hearts";
                case PlayType.TrumpPikes:
                    return "Pikes";
                case PlayType.TrumpTiles:
                    return "Tiles";
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