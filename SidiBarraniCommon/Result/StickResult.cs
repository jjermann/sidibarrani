using SidiBarraniCommon.Info;

namespace SidiBarraniCommon.Result
{
    public class StickResult
    {
        public PlayerInfo Winner { get; set; }

        public override string ToString()
        {
            var str = $"{Winner} won the stick.";
            return str;
        }
    }
}