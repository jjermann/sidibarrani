using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SidiBarraniCommon;
using SidiBarraniCommon.Info;

namespace SidiBarraniAi
{
    public class SidiBarraniRandomClient : ISidiBarraniClientApi
    {
        private ISidiBarraniServerApi SidiBarraniServerApi {get;}
        private int DelayInMiliseconds {get;}
        private string GameId {get;set;}
        private string PlayerId {get;set;}

        public SidiBarraniRandomClient(ISidiBarraniServerApi sidiBarraniServerApi, int delayInMiliseconds = 0)
        {
            SidiBarraniServerApi = sidiBarraniServerApi;
            DelayInMiliseconds = delayInMiliseconds;
        }

        public bool RequestConfirm() => true;

        public bool SetPlayerGameInfo(PlayerGameInfo playerGameInfo)
        {
            var randomActionId = playerGameInfo?.ValidActionList
                ?.OrderBy(a => Guid.NewGuid())
                ?.FirstOrDefault()
                ?.ActionId;
            if (!randomActionId.HasValue)
            {
                return true;
            }
            Task.Run(() => 
            {
                if (DelayInMiliseconds > 0)
                {
                    Thread.Sleep(DelayInMiliseconds);
                }
                ProcessAction(randomActionId.Value);
            });
            return true;
        }

        public bool ConnectToGame(string gameId, string playerName)
        {
            var playerInfo = SidiBarraniServerApi?.ConnectToGame(gameId, playerName, this);
            if (playerInfo == null)
            {
                return false;
            }
            GameId = gameId;
            PlayerId = playerInfo.PlayerId;
            return true;
        }

        public bool ProcessAction(int actionId)
        {
            var actionInfo = new ActionInfo
            {
                GameId = GameId,
                PlayerId = PlayerId,
                ActionId = actionId
            };
            return SidiBarraniServerApi?.ProcessAction(actionInfo) ?? false;
        }

        public override string ToString()
        {
            return $"SidiBarraniRandomClient (GameId={GameId}, PlayerId={PlayerId})";
        }
    }
}
