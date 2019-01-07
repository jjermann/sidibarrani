using System.Collections.Generic;

namespace SidiBarrani.Model
{
    public class CardComparer : IComparer<Card>
    {
        private CardComparer() {}
        private PlayType ActivePlayType {get;set;}
        private CardSuit ActiveSuit {get;set;}

        public CardComparer(PlayType playType, CardSuit activeSuit)
        {
            ActivePlayType = playType;
            ActiveSuit = activeSuit;
        }

        public int Compare(Card x, Card y)
        {
            if (x.CardSuit == y.CardSuit) {
                return x.GetRank(ActivePlayType).CompareTo(y.GetRank(ActivePlayType));
            }
            else
            {
                if (ActivePlayType.IsTrump()) {
                    if (x.CardSuit == ActivePlayType.GetTrumpSuit())
                    {
                        return 1;
                    }
                    if (y.CardSuit == ActivePlayType.GetTrumpSuit())
                    {
                        return -1;
                    }
                }
                if (x.CardSuit == ActiveSuit) {
                    return 1;
                }
                if (y.CardSuit == ActiveSuit) {
                    return -1;
                }
                return 0;
            }
        }
    }
}