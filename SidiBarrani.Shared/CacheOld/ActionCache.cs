using System;
using System.Collections.Generic;
using System.Linq;
using SidiBarraniCommon.ActionOld;
using SidiBarraniCommon.InfoOld;
using SidiBarraniCommon.Model;

namespace SidiBarraniCommon.CacheOld
{
    public class ActionCache
    {
        private readonly Rules _rules;
        private readonly IDictionary<int, BetAction> _betDictionary;
        private readonly IDictionary<int, PlayAction> _playDictionary;

        public ActionCache(Rules rules)
        {
            _rules = rules;
            _betDictionary = GetBetActionList().ToDictionary(a => a.GetActionId(), a => a);
            _playDictionary = GetPlayActionList().ToDictionary(a => a.GetActionId(), a => a);
        }

        public ActionBase? ConstructAction(GameInfo gameInfo, PlayerInfo playerInfo, int actionId)
        {
            if (_betDictionary.ContainsKey(actionId))
            {
                var betAction = _betDictionary[actionId] with
                {
                    GameInfo = gameInfo,
                    PlayerInfo = playerInfo
                };
                return betAction;
            }
            if (_playDictionary.ContainsKey(actionId))
            {
                var playAction = _playDictionary[actionId] with
                {
                    GameInfo = gameInfo,
                    PlayerInfo = playerInfo
                };
                return playAction;
            }
            return null;
        }

        private IList<BetAction> GetBetActionList()
        {
            var betActionList = new List<BetAction>
            {
                new(BetActionType.Pass),
                new(BetActionType.Sidi),
                new(BetActionType.Barrani)
            };
            var betActionBetList = _rules.GetValidBets()
                .Select(bet => new BetAction(BetActionType.Bet, bet))
                .ToList();
            betActionList.AddRange(betActionBetList);
            return betActionList;
        }

        private IList<PlayAction> GetPlayActionList()
        {
            var playActionList = new List<PlayAction>();
            foreach (var cardSuit in Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>())
            {
                foreach (var cardRank in Enum.GetValues(typeof(CardRank)).Cast<CardRank>())
                {
                    var playAction = new PlayAction(new Card(cardSuit, cardRank));
                    playActionList.Add(playAction);
                }
            }
            return playActionList;
        }
    }
}