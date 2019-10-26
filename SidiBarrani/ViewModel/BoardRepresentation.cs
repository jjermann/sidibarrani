using System.Linq;
using ReactiveUI;
using SidiBarraniCommon.Info;

namespace SidiBarrani.ViewModel
{
    public class BoardRepresentation : ReactiveObject
    {
        private GameInfo GameInfo {get;}
        private PlayerInfo PlayerInfo {get;}
        private GameStageInfo GameStageInfo {get;}
        public CardRepresentation TopCardRepresentation {get;}
        public CardRepresentation LeftCardRepresentation {get;}
        public CardRepresentation RightCardRepresentation {get;}
        public CardRepresentation BottomCardRepresentation {get;}
        public BetResultRepresentation BetResultRepresentation {get;}

        public BoardRepresentation(GameInfo gameInfo, PlayerInfo playerInfo, GameStageInfo gameStageInfo)
        {
            GameInfo = gameInfo;
            PlayerInfo = playerInfo;
            GameStageInfo = gameStageInfo;
            var betResult = GameStageInfo?.CurrentBetResult;
            BetResultRepresentation = betResult != null
                ? new BetResultRepresentation(betResult)
                : null;

            var topPlayer = GameInfo.PlayerGroupInfo.GetOppositePlayer(PlayerInfo.PlayerId);
            var rightPlayer = GameInfo.PlayerGroupInfo.GetNextPlayer(PlayerInfo.PlayerId);
            var leftPlayer = GameInfo.PlayerGroupInfo.GetPreviousPlayer(PlayerInfo.PlayerId);
            var topCard = GameStageInfo?.CurrentStickRoundInfo?.PlayActionList
                ?.SingleOrDefault(a => a.PlayerInfo.PlayerId == topPlayer.PlayerId)
                ?.Card;
            var rightCard = GameStageInfo?.CurrentStickRoundInfo?.PlayActionList
                ?.SingleOrDefault(a => a.PlayerInfo.PlayerId == rightPlayer.PlayerId)
                ?.Card;
            var leftCard = GameStageInfo?.CurrentStickRoundInfo?.PlayActionList
                ?.SingleOrDefault(a => a.PlayerInfo.PlayerId == leftPlayer.PlayerId)
                ?.Card;
            var bottomCard = GameStageInfo?.CurrentStickRoundInfo?.PlayActionList
                ?.SingleOrDefault(a => a.PlayerInfo.PlayerId == PlayerInfo.PlayerId)
                ?.Card;
            TopCardRepresentation = topCard != null
                ? new CardRepresentation(topCard)
                : null;
            LeftCardRepresentation = leftCard != null
                ? new CardRepresentation(leftCard)
                : null;
            RightCardRepresentation = rightCard != null
                ? new CardRepresentation(rightCard)
                : null;
            BottomCardRepresentation = bottomCard != null
                ? new CardRepresentation(bottomCard)
                : null;
        }
    }
}