using System.Collections.Generic;
using System.Linq;

namespace SidiBarrani.Model
{
    public class BetStage
    {
        public BetStage()
        {
            BetActionList = new List<BetAction>();
        }
        private IList<BetAction> BetActionList {get;set;}

        private BetAction GetLastBetAction()
        {
            return BetActionList.LastOrDefault(b => b.Type == BetActionType.Bet);
        }
        public BetResult GetBetResult()
        {
            var lastBetAction = GetLastBetAction();
            if (lastBetAction == null)
            {
                return null;
            }
            var lastActions = BetActionList.TakeLast(4).ToList();
            var barraniBetAction = lastActions.SingleOrDefault(a => a.Type == BetActionType.Barrani);
            if (barraniBetAction != null)
            {
                return new BetResult
                {
                    BettingTeam = lastBetAction.Player.Team,
                    Bet = lastBetAction.Bet,
                    IsSidi = true,
                    IsBarrani = true
                };
            }
            var sidiBetAction = lastActions.SingleOrDefault(a => a.Type == BetActionType.Sidi);
            if (sidiBetAction != null)
            {
                var sidiIndex = lastActions.IndexOf(sidiBetAction);
                var followUpCount = lastActions.Count - (sidiIndex+1);
                var followedActions = followUpCount > 0
                    ? lastActions.GetRange(sidiIndex+1, followUpCount)
                    : new List<BetAction>();
                var otherTeamFollowedActions = followedActions.Where(a => a.Player.Team != sidiBetAction.Player.Team);
                if (otherTeamFollowedActions.Any(a => a.Type != BetActionType.Pass))
                {
                    throw new System.InvalidOperationException();
                }
                var passingOpponents = otherTeamFollowedActions.Select(a => a.Player).Distinct().ToList();
                if (passingOpponents.Count == 2)
                {
                    return new BetResult
                    {
                        BettingTeam = lastBetAction.Player.Team,
                        Bet = lastBetAction.Bet,
                        IsSidi = true
                    };
                }
                else
                {
                    return null;
                }
            }

            if (lastActions.Count < 4)
            {
                return null;
            }
            if (lastActions.TakeLast(3).All(b => b.Type == BetActionType.Pass))
            {
                return new BetResult
                {
                    BettingTeam = lastBetAction.Player.Team,
                    Bet = lastBetAction.Bet,
                };
            }
            return null;
        }
    }
}