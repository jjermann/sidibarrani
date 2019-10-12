using System;

namespace SidiBarraniCommon.Info
{
    public class PlayerGroupInfo : ICloneable
    {
        public TeamInfo Team1 { get; set; }
        public TeamInfo Team2 { get; set; }

        public object Clone()
        {
            return new PlayerGroupInfo
            {
                Team1 = (TeamInfo)Team1?.Clone(),
                Team2 = (TeamInfo)Team2?.Clone()
            };
        }

    }
}