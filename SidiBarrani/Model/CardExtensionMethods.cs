using System;

namespace SidiBarrani.Model
{
    public static class CardExtensionMethods
    {
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
                        throw new InvalidOperationException();
                }
            }
            else
            {
                var isUpDown = playType == PlayType.UpDown;
                var isDownUp = playType == PlayType.DownUp;
                switch(card.CardRank)
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
                        throw new InvalidOperationException();
                }
            }
        }
    }
}