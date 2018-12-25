using System;
using System.Collections.Generic;
using System.Linq;

namespace SidiBarrani.Model
{
    public class BetStage
    {
        public BetStage(Rules rules, PlayerGroup playerGroup, Player initialPlayer)
        {
            Rules = rules;
            PlayerGroup = playerGroup;
            CurrentPlayer = initialPlayer;
            if (!PlayerGroup.GetPlayerList().Contains(initialPlayer)) {
                throw new ArgumentException();
            }
            BetActionList = new List<BetAction>();
        }
        private Rules Rules {get;set;}
        public Player CurrentPlayer {get; private set;}
        private PlayerGroup PlayerGroup {get;}
        private List<BetAction> BetActionList {get;}

        private BetAction GetLastBetBetAction()
        {
            return BetActionList.LastOrDefault(b => b.Type == BetActionType.Bet);
        }
        private BetAction GetLastNonPassBetAction()
        {
            return BetActionList.LastOrDefault(b => b.Type != BetActionType.Pass);
        }

        public BetResult GetBetResult()
        {
            var lastBetBetAction = GetLastBetBetAction();
            if (lastBetBetAction == null)
            {
                return null;
            }
            var followedActions = GetFollowedBetActions(lastBetBetAction);
            var barraniBetAction = followedActions.SingleOrDefault(a => a.Type == BetActionType.Barrani);
            if (barraniBetAction != null)
            {
                //Case Barrani occured
                return new BetResult
                {
                    BettingTeam = lastBetBetAction.Player.Team,
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
                if (sidiFollowedActions.Any(a => a.Type != BetActionType.Pass || a.Player.Team == sidiBetAction.Player.Team))
                {
                    throw new System.InvalidOperationException();
                }
                var passingOpponents = sidiFollowedActions.Select(a => a.Player).Distinct().ToList();
                if (passingOpponents.Count == 2)
                {
                    return new BetResult
                    {
                        BettingTeam = lastBetBetAction.Player.Team,
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
            var tooViewFollowedActions = (!isGeneralBet && followedActions.Count < 3)
                || (isGeneralBet && followedActions.Count < 2);
            if (tooViewFollowedActions)
            {
                return null;
            }
            if (followedActions.All(b => b.Type == BetActionType.Pass))
            {
                return new BetResult
                {
                    BettingTeam = lastBetBetAction.Player.Team,
                    Bet = lastBetBetAction.Bet,
                };
            }
            return null;
        }

        private IList<BetAction> GetFollowedBetActions(BetAction betAction)
        {
            var betActionIndex = BetActionList.IndexOf(betAction);
            var followUpCount = BetActionList.Count - (betActionIndex+1);
            var followedActions = followUpCount > 0
                ? BetActionList.GetRange(betActionIndex+1, followUpCount)
                : new List<BetAction>();
            return followedActions;
        }

        public IList<Bet> GetValidNewBets()
        {
            var currentBet = GetLastBetBetAction()?.Bet;
            var allBets = Rules.GetValidBets();
            var result = allBets
                .Where(b => currentBet == null || b.CompareTo(currentBet) > 0)
                .ToList();
            return result;
        }

        public IList<BetAction> GetValidBetActions()
        {
            var lastBetAction = GetLastNonPassBetAction();
            if (lastBetAction != null && lastBetAction.Type == BetActionType.Barrani)
            {
                //Case Barrani occured
                return new List<BetAction>();
            }
            if (lastBetAction != null && lastBetAction.Type == BetActionType.Sidi)
            {
                //Case Sidi occured
                var opposingTeam = PlayerGroup.GetOtherTeam(lastBetAction.Player.Team);
                var alreadyPassed = GetFollowedBetActions(lastBetAction)
                    .Select(a => a.Player)
                    .ToList();
                var betActionList = new List<BetAction>();
                var nextOpposingPlayer = PlayerGroup
                    .GetPlayerListFromInitialPlayer(CurrentPlayer)
                    .First(p => p.Team == opposingTeam);
                if (!alreadyPassed.Contains(opposingTeam.Player1))
                {
                    betActionList.Add(new BetAction
                    {
                        Player = opposingTeam.Player1,
                        Type = BetActionType.Barrani
                    });
                    if (nextOpposingPlayer == opposingTeam.Player1)
                    {
                        betActionList.Add(new BetAction
                        {
                            Player = opposingTeam.Player1,
                            Type = BetActionType.Pass
                        });
                    }
                }
                if (!alreadyPassed.Contains(opposingTeam.Player2))
                {
                    betActionList.Add(new BetAction
                    {
                        Player = opposingTeam.Player2,
                        Type = BetActionType.Barrani
                    });
                    if (nextOpposingPlayer == opposingTeam.Player2)
                    {
                        betActionList.Add(new BetAction
                        {
                            Player = opposingTeam.Player2,
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
                    Player = CurrentPlayer,
                    Type = BetActionType.Bet,
                    Bet = b
                })
                .ToList();

            var generalBetAction = lastBetAction?.Bet != null && lastBetAction.Bet.BetAmount.IsGeneral
                ? lastBetAction
                : null;
            if (generalBetAction == null || CurrentPlayer.Team != generalBetAction.Player.Team)
            {
                validBetActions.Add(new BetAction
                {
                    Player = CurrentPlayer,
                    Type = BetActionType.Pass
                });
            }
            if (lastBetAction != null)
            {
                var opposingTeam = PlayerGroup.GetOtherTeam(lastBetAction.Player.Team);
                var passingOppositionPlayer = GetFollowedBetActions(lastBetAction)
                    .Where(a => a.Player.Team == opposingTeam && a.Type == BetActionType.Pass)
                    .Select(a => a.Player)
                    .ToList();
                if (!passingOppositionPlayer.Contains(opposingTeam.Player1))
                {
                    validBetActions.Add(new BetAction
                    {
                        Player = opposingTeam.Player1,
                        Type = BetActionType.Sidi
                    });
                }
                if (!passingOppositionPlayer.Contains(opposingTeam.Player2))
                {
                    validBetActions.Add(new BetAction
                    {
                        Player = opposingTeam.Player2,
                        Type = BetActionType.Sidi
                    });
                }
            }
            return validBetActions;
        }

        public bool AddBetActionAndProceed(BetAction betAction = null)
        {
            if (betAction == null)
            {
                CurrentPlayer = PlayerGroup.GetNextPlayer(CurrentPlayer);
                return true;
            }
            var validBetActions = GetValidBetActions();
            if (!validBetActions.Contains(betAction)) 
            {
                return false;
            }
            BetActionList.Add(betAction);
            if (betAction.Player == CurrentPlayer)
            {
                CurrentPlayer = PlayerGroup.GetNextPlayer(CurrentPlayer);
            }
            return true;
        }

        public override string ToString()
        {
            var str = string.Join(Environment.NewLine, BetActionList.Select(a => $"BetAction: {a}"));
            var betResult = GetBetResult();
            if (betResult != null)
            {
                str += Environment.NewLine;
                str += $"BetResult: {betResult}" + Environment.NewLine;
            }
            return str;
        }
    }
}