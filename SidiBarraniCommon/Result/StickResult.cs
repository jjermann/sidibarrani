using System;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;

namespace SidiBarraniCommon.Result
{
    public class StickResult : ICloneable
    {
        public PlayerInfo Winner { get; set; }
        public CardPile StickPile {get;set;}

        public object Clone()
        {
            return new StickResult
            {
                Winner = (PlayerInfo)Winner?.Clone(),
                StickPile = (CardPile)StickPile?.Clone()
            };
        }

        public override string ToString()
        {
            var str = $"{Winner} won the stick.";
            return str;
        }
    }
}