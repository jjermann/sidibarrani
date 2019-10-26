using System;
using System.Collections.Generic;
using System.Linq;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;

namespace SidiBarraniCommon.Result
{
    public class StickResult : ICloneable
    {
        public PlayerInfo Winner { get; set; }
        public IList<Card> StickPile {get;set;}

        public object Clone()
        {
            return new StickResult
            {
                Winner = (PlayerInfo)Winner?.Clone(),
                StickPile = StickPile?.Select(c => (Card)c?.Clone()).ToList()
            };
        }

        public override string ToString()
        {
            var str = $"{Winner} won the stick.";
            return str;
        }
    }
}