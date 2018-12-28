using System;
using System.Collections.Generic;
using System.Linq;
using DynamicData;
using ReactiveUI;

namespace SidiBarrani.Model
{
    public class StickRound : ReactiveObject
    {
        private StickRound() { }
        public StickRound(Rules rules, PlayerGroup playerGroup, Player initialPlayer, PlayType playType) {
            Rules = rules;
            PlayerGroup = playerGroup;
            CurrentPlayer = initialPlayer;
            PlayType = playType;

            PlayActionSourceList = new SourceList<PlayAction>();
            PlayActionList = PlayActionSourceList.AsObservableList();
        }

        private Rules Rules {get;set;}
        private PlayerGroup PlayerGroup {get;}
        private PlayType PlayType {get;}
        private SourceList<PlayAction> PlayActionSourceList {get;}
        public IObservableList<PlayAction> PlayActionList {get;}

        private Player _currentPlayer;
        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            private set { this.RaiseAndSetIfChanged(ref _currentPlayer, value); }
        }
        public CardSuit? StickSuit => PlayActionSourceList.Items.FirstOrDefault()?.Card.CardSuit;

        private CardComparer GetCardComparer()
        {
            if (!StickSuit.HasValue)
            {
                throw new InvalidOperationException();
            }
            var cardComparer = new CardComparer(PlayType, StickSuit.Value);
            return cardComparer;
        }
        private StickPile GetStickPile()
        {
            var stickPile = new StickPile
            {
                Cards = PlayActionSourceList.Items.Select(a => a.Card).ToList()
            };
            return stickPile;
        }

        public StickResult GetStickResult()
        {
            if (!StickSuit.HasValue || PlayActionSourceList.Count != 4)
            {
                return null;
            }
            var winner = PlayActionSourceList
                .Items
                .OrderByDescending(a => a.Card, GetCardComparer())
                .First()
                .Player;
            var stickResult = new StickResult
            {
                Winner = winner,
                StickPile = GetStickPile()
            };
            return stickResult;
        }

        public IList<PlayAction> GetValidPlayActions()
        {
            var allHandPlayActionList = CurrentPlayer.Context.CardsInHand
                .Select(c => new PlayAction
                {
                    Player = CurrentPlayer,
                    Card = c
                })
                .ToList();
            if (!StickSuit.HasValue)
            {
                //Case nothing played yet
                return allHandPlayActionList;
            }
            var cardComparer = GetCardComparer();
            var highestPlayedCard = PlayActionSourceList
                .Items
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
        public bool AddPlayActionAndProceed(PlayAction playAction)
        {
            var validPlayActions = GetValidPlayActions();
            if (!validPlayActions.Contains(playAction))
            {
                return false;
            }
            CurrentPlayer.Context.CardsInHand.Remove(playAction.Card);
            PlayActionSourceList.Add(playAction);
            CurrentPlayer = PlayerGroup.GetNextPlayer(CurrentPlayer);
            return true;
        }
    }
}