using System;
using System.Collections.Generic;

namespace SidiBarrani.Shared.Model.Game
{
    public static class CardExtensionMethods
    {
        public static int GetRank(this Card card, PlayType playType)
        {
            if (playType.IsTrump() && card.CardSuit == playType.GetTrumpSuit())
            {
                return 10 + TrumpRankList.IndexOf(card.CardRank);
            }
            if (playType.IsTrump() || playType == PlayType.UpDown)
            {
                return UpDownRankList.IndexOf(card.CardRank);
            }
            return DownUpRankList.IndexOf(card.CardRank);
        }

        private static readonly IList<CardRank> TrumpRankList = new List<CardRank>
        {
            CardRank.Six,
            CardRank.Seven,
            CardRank.Eight,
            CardRank.Ten,
            CardRank.Queen,
            CardRank.King,
            CardRank.Ace,
            CardRank.Nine,
            CardRank.Jack
        };
        private static readonly IList<CardRank> UpDownRankList = new List<CardRank>
        {
            CardRank.Six,
            CardRank.Seven,
            CardRank.Eight,
            CardRank.Nine,
            CardRank.Ten,
            CardRank.Jack,
            CardRank.Queen,
            CardRank.King,
            CardRank.Ace
        };
        private static readonly IList<CardRank> DownUpRankList = new List<CardRank>
        {
            CardRank.Ace,
            CardRank.King,
            CardRank.Queen,
            CardRank.Jack,
            CardRank.Ten,
            CardRank.Nine,
            CardRank.Eight,
            CardRank.Seven,
            CardRank.Six
        };

        public static int GetValue(this Card card, PlayType playType)
        {
            if (playType.IsTrump())
            {
                var isTrumpCard = card.CardSuit == playType.GetTrumpSuit();
                switch (card.CardRank)
                {
                    case CardRank.Six:
                        return 0;
                    case CardRank.Seven:
                        return 0;
                    case CardRank.Eight:
                        return 0;
                    case CardRank.Nine:
                        return isTrumpCard
                            ? 14
                            : 0;
                    case CardRank.Ten:
                        return 10;
                    case CardRank.Jack:
                        return isTrumpCard
                            ? 20
                            : 2;
                    case CardRank.Queen:
                        return 3;
                    case CardRank.King:
                        return 4;
                    case CardRank.Ace:
                        return 11;
                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                var isUpDown = playType == PlayType.UpDown;
                var isDownUp = playType == PlayType.DownUp;
                switch (card.CardRank)
                {
                    case CardRank.Six:
                        return isDownUp
                            ? 11
                            : 0;
                    case CardRank.Seven:
                        return 0;
                    case CardRank.Eight:
                        return 8;
                    case CardRank.Nine:
                        return 0;
                    case CardRank.Ten:
                        return 10;
                    case CardRank.Jack:
                        return 2;
                    case CardRank.Queen:
                        return 3;
                    case CardRank.King:
                        return 4;
                    case CardRank.Ace:
                        return isUpDown
                            ? 11
                            : 0;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}