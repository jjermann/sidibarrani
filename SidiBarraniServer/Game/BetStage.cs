using System;
using System.Collections.Generic;
using System.Linq;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;
using SidiBarraniCommon.Result;

namespace SidiBarraniServer.Game
{
    public class BetStage
    {
        private Rules Rules { get; }
        private PlayerGroupInfo PlayerGroup { get; }
        private IDictionary<string,CardPile> PlayerHandDictionary {get;}
        private PlayerInfo CurrentPlayer { get; set; }
        
        public IList<BetAction> BetActionList {get;set;} = new List<BetAction>();
        public BetResult BetResult {get;set;}

        public BetStage(Rules rules, PlayerGroupInfo playerGroup, IDictionary<string,CardPile> playerHandDictionary, PlayerInfo initialPlayer)
        {
            Rules = rules;
            PlayerGroup = playerGroup;
            PlayerHandDictionary = playerHandDictionary;
            CurrentPlayer = initialPlayer;
        }

        private BetAction GetLastBetBetAction()
        {
            return BetActionList.LastOrDefault(b => b.Type == BetActionType.Bet);
        }

        private BetAction GetLastNonPassBetAction()
        {
            return BetActionList.LastOrDefault(b => b.Type != BetActionType.Pass);
        }

        private IList<BetAction> GetFollowedBetActions(BetAction betAction)
        {
            var betActionIndex = BetActionList.IndexOf(betAction);
            var followUpCount = BetActionList.Count - (betActionIndex + 1);
            var followedActions = followUpCount > 0
                ? BetActionList.ToList().GetRange(betActionIndex + 1, followUpCount)
                : new List<BetAction>();
            return followedActions;
        }

        private BetResult GetBetResult()
        {
            var lastBetBetAction = GetLastBetBetAction();
            if (lastBetBetAction == null)
            {
                return null;
            }
            var bettingTeam = PlayerGroup.GetTeamOfPlayer(lastBetBetAction.PlayerInfo.PlayerId);
            var followedActions = GetFollowedBetActions(lastBetBetAction);
            var barraniBetAction = followedActions.SingleOrDefault(a => a.Type == BetActionType.Barrani);
            if (barraniBetAction != null)
            {
                //Case Barrani occured
                return new BetResult
                {
                    BettingTeam = bettingTeam,
                    Bet = lastBetBetAction.Bet,
                    IsSidi = true,
                    IsBarrani = true
                };
            }
            var sidiBetAction = followedActions.SingleOrDefault(a => a.Type == BetActionType.Sidi);
            if (sidiBetAction != null)
            {
                //Case Sidi occured
                var sidiFollowedActions = GetFollowedBetActions(sidiBetAction);
                if (sidiFollowedActions.Any(a => 
                    a.Type != BetActionType.Pass
                    || PlayerGroup.GetTeamOfPlayer(a.PlayerInfo.PlayerId) == PlayerGroup.GetTeamOfPlayer(sidiBetAction.PlayerInfo.PlayerId)))
                {
                    throw new InvalidOperationException();
                }
                var passingOpponents = sidiFollowedActions.Select(a => a.PlayerInfo).Distinct().ToList();
                if (passingOpponents.Count == 2)
                {
                    return new BetResult
                    {
                        BettingTeam = bettingTeam,
                        Bet = lastBetBetAction.Bet,
                        IsSidi = true
                    };
                }
                else
                {
                    return null;
                }
            }

            //Case no Sidi/Barrani occured (followedActions are all Pass)
            var isGeneralBet = lastBetBetAction?.Bet != null && lastBetBetAction.Bet.BetAmount.IsGeneral;
            var tooViewFollowedActions = !isGeneralBet && followedActions.Count < 3
                || isGeneralBet && followedActions.Count < 2;
            if (tooViewFollowedActions)
            {
                return null;
            }
            if (followedActions.All(b => b.Type == BetActionType.Pass))
            {
                return new BetResult
                {
                    BettingTeam = bettingTeam,
                    Bet = lastBetBetAction.Bet,
                };
            }
            return null;
        }

        private IList<Bet> GetValidNewBets()
        {
            var currentBet = GetLastBetBetAction()?.Bet;
            var allBets = Rules.GetValidBets();
            var result = allBets
                .Where(b => currentBet == null || b.CompareTo(currentBet) > 0)
                .ToList();
            return result;
        }

        private IList<BetAction> GetValidBetActions()
        {
            var lastBetAction = GetLastNonPassBetAction();
            var betActionTeam = lastBetAction != null
                ? PlayerGroup.GetTeamOfPlayer(lastBetAction.PlayerInfo.PlayerId)
                : null;
            if (lastBetAction != null && lastBetAction.Type == BetActionType.Barrani)
            {
                //Case Barrani occured
                return new List<BetAction>();
            }
            if (lastBetAction != null && lastBetAction.Type == BetActionType.Sidi)
            {
                //Case Sidi occured
                var opposingTeam = PlayerGroup.GetOtherTeam(betActionTeam.TeamId);
                var alreadyPassed = GetFollowedBetActions(lastBetAction)
                    .Select(a => a.PlayerInfo)
                    .ToList();
                var betActionList = new List<BetAction>();
                var nextActiveOpposingPlayer = PlayerGroup
                    .GetPlayerList(CurrentPlayer.PlayerId)
                    .FirstOrDefault(p => PlayerGroup.GetTeamOfPlayer(p.PlayerId) == opposingTeam && !alreadyPassed.Contains(p));
                if (!alreadyPassed.Contains(opposingTeam.Player1))
                {
                    betActionList.Add(new BetAction
                    {
                        PlayerInfo = opposingTeam.Player1,
                        Type = BetActionType.Barrani
                    });
                    if (nextActiveOpposingPlayer == opposingTeam.Player1)
                    {
                        betActionList.Add(new BetAction
                        {
                            PlayerInfo = opposingTeam.Player1,
                            Type = BetActionType.Pass
                        });
                    }
                }
                if (!alreadyPassed.Contains(opposingTeam.Player2))
                {
                    betActionList.Add(new BetAction
                    {
                        PlayerInfo = opposingTeam.Player2,
                        Type = BetActionType.Barrani
                    });
                    if (nextActiveOpposingPlayer == opposingTeam.Player2)
                    {
                        betActionList.Add(new BetAction
                        {
                            PlayerInfo = opposingTeam.Player2,
                            Type = BetActionType.Pass
                        });
                    }
                }
                return betActionList;
            }
            //Case no Sidi/Barrani occured (lastBetAction is a Bet or null)
            var validNewBets = GetValidNewBets();
            var validBetActions = validNewBets
                .Select(b => new BetAction
                {
                    PlayerInfo = CurrentPlayer,
                    Type = BetActionType.Bet,
                    Bet = b
                })
                .ToList();

            var generalBetAction = lastBetAction?.Bet != null && lastBetAction.Bet.BetAmount.IsGeneral
                ? lastBetAction
                : null;
            if (generalBetAction != null)
            {
                var opposingTeam = PlayerGroup.GetOpposingTeamOfPlayer(generalBetAction.PlayerInfo.PlayerId);
                var alreadyPassed = GetFollowedBetActions(generalBetAction)
                    .Select(a => a.PlayerInfo)
                    .ToList();
                var nextActiveOpposingPlayer = PlayerGroup
                    .GetPlayerList(CurrentPlayer.PlayerId)
                    .FirstOrDefault(p => PlayerGroup.GetTeamOfPlayer(p.PlayerId) == opposingTeam && !alreadyPassed.Contains(p));
                if (nextActiveOpposingPlayer != null)
                {
                    validBetActions.Add(new BetAction
                    {
                        PlayerInfo = nextActiveOpposingPlayer,
                        Type = BetActionType.Pass
                    });
                }
            }
            else
            {
                validBetActions.Add(new BetAction
                {
                    PlayerInfo = CurrentPlayer,
                    Type = BetActionType.Pass
                });
            }
            if (lastBetAction != null)
            {
                var opposingTeam = PlayerGroup.GetOpposingTeamOfPlayer(lastBetAction.PlayerInfo.PlayerId);
                var passingOppositionPlayer = GetFollowedBetActions(lastBetAction)
                    .Where(a => PlayerGroup.GetTeamOfPlayer(a.PlayerInfo.PlayerId) == opposingTeam && a.Type == BetActionType.Pass)
                    .Select(a => a.PlayerInfo)
                    .ToList();
                if (!passingOppositionPlayer.Contains(opposingTeam.Player1))
                {
                    validBetActions.Add(new BetAction
                    {
                        PlayerInfo = opposingTeam.Player1,
                        Type = BetActionType.Sidi
                    });
                }
                if (!passingOppositionPlayer.Contains(opposingTeam.Player2))
                {
                    validBetActions.Add(new BetAction
                    {
                        PlayerInfo = opposingTeam.Player2,
                        Type = BetActionType.Sidi
                    });
                }
            }
            return validBetActions;
        }

        public IList<int> GetValidActionIdList(string playerId)
        {
            if (BetResult != null)
            {
                return new List<int>();
            }
            var validBetActionList = GetValidBetActions();
            return validBetActionList
                .Where(a => a.PlayerInfo.PlayerId == playerId)
                .Select(a => a.GetActionId())
                .ToList();
        }

        public void ProcessBetAction(BetAction betAction)
        {
            if (BetResult != null)
            {
                throw new InvalidOperationException();
            }
            BetActionList.Add(betAction);
            BetResult = GetBetResult();
            if (BetResult == null && betAction.PlayerInfo == CurrentPlayer)
            {
                CurrentPlayer = PlayerGroup.GetNextPlayer(CurrentPlayer.PlayerId);
            }
        }

        public override string ToString()
        {
            var str = string.Join(Environment.NewLine, BetActionList.Select(a => $"BetAction: {a}"));
            if (BetResult != null)
            {
                str += Environment.NewLine;
                str += $"BetResult: {BetResult}" + Environment.NewLine;
            }
            return str;
        }
    }
}