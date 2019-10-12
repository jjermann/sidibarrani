using System;
using System.Collections.Generic;
using System.Linq;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;

namespace SidiBarraniCommon.Cache
{
    public class ActionCache
    {
        private readonly Rules _rules;
        private readonly IDictionary<int, SetupAction> _setupDictionary;
        private readonly IDictionary<int, BetAction> _betDictionary;
        private readonly IDictionary<int, PlayAction> _playDictionary;
        private readonly IDictionary<int, ConfirmAction> _confirmDictionary;

        public ActionCache(Rules rules)
        {
            _rules = rules;
            _setupDictionary = GetSetupActionList().ToDictionary(a => a.GetActionId(), a => a);
            _betDictionary = GetBetActionList().ToDictionary(a => a.GetActionId(), a => a);
            _playDictionary = GetPlayActionList().ToDictionary(a => a.GetActionId(), a => a);
            _confirmDictionary = GetConfirmActionList().ToDictionary(a => a.GetActionId(), a => a);
        }

        public ActionBase ConstructAction(GameInfo gameInfo, PlayerInfo playerInfo, int actionId)
        {
            if (_setupDictionary.ContainsKey(actionId))
            {
                var setupAction = (SetupAction)_setupDictionary[actionId].Clone();
                setupAction.GameInfo = gameInfo;
                setupAction.PlayerInfo = playerInfo;
                return setupAction;
            }
            if (_betDictionary.ContainsKey(actionId))
            {
                var betAction = (BetAction)_betDictionary[actionId].Clone();
                betAction.GameInfo = gameInfo;
                betAction.PlayerInfo = playerInfo;
                return betAction;
            }
            if (_playDictionary.ContainsKey(actionId))
            {
                var playAction = (PlayAction)_playDictionary[actionId].Clone();
                playAction.GameInfo = gameInfo;
                playAction.PlayerInfo = playerInfo;
                return playAction;
            }
            if (_confirmDictionary.ContainsKey(actionId))
            {
                var confirmAction = (ConfirmAction)_confirmDictionary[actionId].Clone();
                confirmAction.GameInfo = gameInfo;
                confirmAction.PlayerInfo = playerInfo;
                return confirmAction;
            }
            return null;
        }

        private IList<SetupAction> GetSetupActionList()
        {
            return new List<SetupAction>
            {
                new SetupAction
                {
                    Setup = true
                },
                new SetupAction
                {
                    Setup = false
                }
            };
        }

        private IList<BetAction> GetBetActionList()
        {
            var betActionList = new List<BetAction>
            {
                new BetAction
                {
                    Type = BetActionType.Pass
                },
                new BetAction
                {
                    Type = BetActionType.Sidi
                },
                new BetAction
                {
                    Type = BetActionType.Barrani
                }
            };
            var betActionBetList = _rules.GetValidBets()
                .Select(bet => new BetAction
                {
                    Type = BetActionType.Bet,
                    Bet = bet
                })
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
                    playActionList.Add(new PlayAction
                    {
                        Card = new Card
                        {
                            CardSuit = cardSuit,
                            CardRank = cardRank
                        }
                    });
                }
            }
            return playActionList;
        }

        private IList<ConfirmAction> GetConfirmActionList()
        {
            return new List<ConfirmAction>
            {
                new ConfirmAction()
            };
        }
    }
}