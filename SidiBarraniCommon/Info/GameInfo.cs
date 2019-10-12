using System;

namespace SidiBarraniCommon.Info
{
    public class GameInfo : ICloneable
    {
        public string GameName { get; set; }
        public string GameId { get; set; }
        public PlayerGroupInfo PlayerGroupInfo {get;set;}

        public object Clone()
        {
            return new GameInfo
            {
                GameName = GameName,
                GameId = GameId,
                PlayerGroupInfo = (PlayerGroupInfo)PlayerGroupInfo?.Clone()
            };
        }

        public override string ToString() => GameName;
    }
}
