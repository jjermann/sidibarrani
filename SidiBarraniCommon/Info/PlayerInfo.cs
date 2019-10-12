using System;

namespace SidiBarraniCommon.Info
{
    public class PlayerInfo : ICloneable
    {
        public string PlayerName { get; set; }
        public string PlayerId { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString() => PlayerName;
    }
}
