using System;
using System.Collections.Generic;
using System.Linq;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Model;
using SidiBarraniCommon.Result;

namespace SidiBarraniCommon.Info
{
    public class StickRoundInfo : ICloneable
    {
        public IList<PlayAction> PlayActionList {get;set;}
        public StickResult StickResult {get;set;}

        public CardSuit? StickSuit => PlayActionList?.FirstOrDefault()?.Card.CardSuit;

        public object Clone()
        {
            return new StickRoundInfo
            {
                PlayActionList = PlayActionList
                    ?.Select(p => (PlayAction)p?.Clone())?.ToList(),
                StickResult = (StickResult)StickResult?.Clone()
            };
        }
    }
}
