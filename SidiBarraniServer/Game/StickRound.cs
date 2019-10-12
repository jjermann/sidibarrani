using System.Collections.Generic;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Result;

namespace SidiBarraniServer.Game
{
    public class StickRound
    {
        public IList<PlayAction> PlayActionList {get;set;}
        public StickResult StickResult {get;set;}

        public IList<int> GetValidActionIdList(string playerId)
        {
            return new List<int>();
        }
        public void ProcessPlayAction(PlayAction playAction)
        {
            PlayActionList.Add(playAction);
        }
    }
}