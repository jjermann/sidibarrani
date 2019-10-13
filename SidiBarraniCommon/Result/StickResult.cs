using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;

namespace SidiBarraniCommon.Result
{
    public class StickResult
    {
        public PlayerInfo Winner { get; set; }
        public CardPile StickPile {get;set;}

        public override string ToString()
        {
            var str = $"{Winner} won the stick.";
            return str;
        }
    }
}