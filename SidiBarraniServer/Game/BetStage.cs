using System.Collections.Generic;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Result;

namespace SidiBarraniServer.Game
{
    public class BetStage
    {
        public IList<BetAction> BetActionList {get;set;} = new List<BetAction>();
        public BetResult BetResult {get;set;}

        public IList<int> GetValidActionIdList(string playerId)
        {
            return new List<int>();
        }

        public void ProcessBetAction(BetAction betAction)
        {
            BetActionList.Add(betAction);
        }
    }
}