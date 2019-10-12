using System;
using System.Collections.Generic;
using System.Linq;

namespace SidiBarraniCommon.Info
{
    public class PlayerGameInfo : ICloneable
    {
        public IList<ActionInfo> ValidActionList { get; set; }

        public object Clone()
        {
            return new PlayerGameInfo
            {
                ValidActionList = ValidActionList != null
                    ? new List<ActionInfo>(ValidActionList.Select(a => (ActionInfo)a.Clone()))
                    : null
            };
        }
    }
}
