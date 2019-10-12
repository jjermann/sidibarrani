using System;

namespace SidiBarraniCommon.Info
{
    public class TeamInfo : ICloneable
    {
        public string TeamName { get; set; }
        public string TeamId {get;set;}
        public PlayerInfo Player1 {get;set;}
        public PlayerInfo Player2 {get;set;}

        public object Clone()
        {
            return new TeamInfo
            {
                TeamName = TeamName,
                TeamId = TeamId,
                Player1 = (PlayerInfo)Player1?.Clone(),
                Player2 = (PlayerInfo)Player2?.Clone()
            };
        }

        public override string ToString() => TeamName;
    }
}
