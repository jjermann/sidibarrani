using System;

namespace SidiBarraniCommon.Info
{
    public class ActionInfo : ICloneable
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }
        public int ActionId { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}