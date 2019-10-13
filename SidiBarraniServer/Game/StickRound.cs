using System;
using System.Collections.Generic;
using System.Linq;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;
using SidiBarraniCommon.Result;

namespace SidiBarraniServer.Game
{
    public class StickRound
    {
        private PlayerGroupInfo PlayerGroupInfo {get;}
        private IDictionary<string,CardPile> PlayerHandDictionary {get;}
        private PlayerInfo CurrentPlayer {get;set;}
        private PlayType PlayType { get; }

        public IList<PlayAction> PlayActionList {get;} = new List<PlayAction>();
        public StickResult StickResult {get;set;}
        public CardSuit? StickSuit => PlayActionList?.FirstOrDefault()?.Card.CardSuit;
        
        public StickRound(PlayerGroupInfo playerGroupInfo, IDictionary<string,CardPile> playerHandDictionary, PlayerInfo initialPlayer, PlayType playType)
        {
            PlayerGroupInfo = playerGroupInfo;
            PlayerHandDictionary = playerHandDictionary;
            CurrentPlayer = initialPlayer;
            PlayType = playType;
        }

        public IList<int> GetValidActionIdList(string playerId)
        {
            if (StickResult != null)
            {
                return new List<int>();
            }
            var validPlayActionList = GetValidPlayActions();
            return validPlayActionList
                .Where(a => a.PlayerInfo.PlayerId == playerId)
                .Select(a => a.GetActionId())
                .ToList();
        }

        public void ProcessPlayAction(PlayAction playAction)
        {
            PlayerHandDictionary[CurrentPlayer.PlayerId].Cards.Remove(playAction.Card);
            PlayActionList.Add(playAction);
            StickResult = GetStickResult();
            if (StickResult == null)
            {
                CurrentPlayer = PlayerGroupInfo.GetNextPlayer(CurrentPlayer.PlayerId);
            }
        }

        private CardComparer GetCardComparer()
        {
            if (!StickSuit.HasValue)
            {
                throw new InvalidOperationException();
            }
            var cardComparer = new CardComparer(PlayType, StickSuit.Value);
            return cardComparer;
        }

        private StickResult GetStickResult()
        {
            if (!StickSuit.HasValue || PlayActionList.Count != 4)
            {
                return null;
            }
            var winner = PlayActionList
                .OrderByDescending(a => a.Card, GetCardComparer())
                .First()
                .PlayerInfo;
            var cardList = PlayActionList
                .Select(a => a.Card)
                .ToList();
            var stickResult = new StickResult
            {
                Winner = winner,
                StickPile = new CardPile(cardList)
            };
            return stickResult;
        }

        private IList<PlayAction> GetValidPlayActions()
        {
            var allHandPlayActionList = PlayerHandDictionary[CurrentPlayer.PlayerId].Cards
                .Select(c => new PlayAction
                {
                    PlayerInfo = CurrentPlayer,
                    Card = c
                })
                .ToList();
            if (!StickSuit.HasValue)
            {
                //Case nothing played yet
                return allHandPlayActionList;
            }
            var cardComparer = GetCardComparer();
            var highestPlayedCard = PlayActionList
                .OrderByDescending(a => a.Card, cardComparer)
                .Select(a => a.Card)
                .First();
            var canUnderTrump = PlayType.IsTrump()
                && allHandPlayActionList
                    .All(a => a.Card.CardSuit == PlayType.GetTrumpSuit())
                && allHandPlayActionList
                    .All(a => a.Card.CardRank == CardRank.Jack || cardComparer.Compare(a.Card, highestPlayedCard) < 0);
            var hasSameSuitCardBesidesTrumpJack = allHandPlayActionList
                .Any(a => a.Card.CardSuit == StickSuit.Value
                    && !(PlayType.IsTrump() && a.Card.CardSuit == PlayType.GetTrumpSuit() && a.Card.CardRank == CardRank.Jack));
            var validPlayActions = allHandPlayActionList
                .Where(a =>
                {
                    var isTrump = PlayType.IsTrump() && a.Card.CardSuit == PlayType.GetTrumpSuit();
                    var isHigherThenPlayedCard = cardComparer.Compare(a.Card, highestPlayedCard) > 0;

                    //disjoint condition list
                    var isSameSuit = a.Card.CardSuit == StickSuit.Value;
                    var isHigherNonSuitTrumpCard = !isSameSuit && isTrump && isHigherThenPlayedCard;
                    var isLowerNonSuitUnderTrumpCard = !isSameSuit && isTrump && !isHigherThenPlayedCard && canUnderTrump;
                    var isPlayableNonSuitNonTrumpCard = !isSameSuit && !hasSameSuitCardBesidesTrumpJack;

                    var isValid = isSameSuit
                        || isHigherNonSuitTrumpCard
                        || isLowerNonSuitUnderTrumpCard
                        || isPlayableNonSuitNonTrumpCard;
                    return isValid;
                })
                .ToList();

            return validPlayActions;
        }
    }
}