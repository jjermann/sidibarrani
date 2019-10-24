using System;
using System.Collections.Generic;
using System.Linq;
using SidiBarraniCommon.Model;

namespace SidiBarraniCommon.Info
{
    public class PlayerGameInfo : ICloneable
    {
        public GameInfo GameInfo {get; set;}
        public PlayerInfo PlayerInfo {get;set;}
        public IList<int> ValidActionIdList { get; set; }
        public IList<Card> PlayerHand {get;set;}
        public GameStageInfo GameStageInfo {get;set;}

        public object Clone()
        {
            return new PlayerGameInfo
            {
                GameInfo = (GameInfo)GameInfo?.Clone(),
                PlayerInfo = (PlayerInfo)PlayerInfo?.Clone(),
                ValidActionIdList = ValidActionIdList?.Select(id => id).ToList(),
                PlayerHand = PlayerHand?.Select(h => (Card)h?.Clone()).ToList(),
                GameStageInfo = (GameStageInfo)GameStageInfo?.Clone()
            };
        }
    }
}
