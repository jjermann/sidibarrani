using System.Collections.Generic;
using System.Linq;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Result;

namespace SidiBarraniServer.Game
{
    public class PlayStage
    {
        public IList<StickRound> StickRoundList {get;set;}
        public PlayResult PlayResult {get;set;}
        public StickRound CurrentStickRound => StickRoundList.LastOrDefault();

        public IList<int> GetValidActionIdList(string playerId)
        {
            if (CurrentStickRound == null)
            {
                return new List<int>();
            }
            return CurrentStickRound.GetValidActionIdList(playerId);
        }

        public void ProcessPlayAction(PlayAction playAction)
        {
            CurrentStickRound.ProcessPlayAction(playAction);
        }
    }
}