using System;
using System.Collections.Generic;
using System.Linq;
using SidiBarraniCommon.Model;

namespace SidiBarraniCommon.Info
{
    public class PlayerGameInfo : ICloneable
    {
        public IList<ActionInfo> ValidActionList { get; set; }
        public CardPile PlayerHand {get;set;}
        public GameStageInfo GameStageInfo {get;set;}

        public object Clone()
        {
            return new PlayerGameInfo
            {
                ValidActionList = ValidActionList
                    ?.Select(a => (ActionInfo)a?.Clone())?.ToList(),
                PlayerHand = (CardPile)PlayerHand?.Clone(),
                GameStageInfo = (GameStageInfo)GameStageInfo?.Clone()
            };
        }
    }
}
