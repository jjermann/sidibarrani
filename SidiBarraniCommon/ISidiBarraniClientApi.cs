using System.Collections.Generic;
using SidiBarraniCommon.Info;

namespace SidiBarraniCommon
{
    public interface ISidiBarraniClientApi
    {
        bool RequestConfirm();
        bool SetPlayerGameInfo(PlayerGameInfo playerGameInfo);
    }
}
