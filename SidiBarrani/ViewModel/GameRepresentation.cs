using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;

namespace SidiBarrani.ViewModel
{
    public class GameRepresentation : ReactiveObject
    {
        private CommandFactory CommandFactory {get;}
        private PlayerGameInfo PlayerGameInfo {get;}
        private IList<PlayAction> PlayActionList {get;}
        private IList<BetAction> BetActionList {get;}
        private IDictionary<Card, ICommand> CardCommandDictionary {get;}
        private IDictionary<BetAction, ICommand> BetActionCommandDictionary {get;}
        private GameStageInfo GameStageInfo => PlayerGameInfo?.GameStageInfo;
        public HandRepresentation HandRepresentation {get;}
        public BetActionsRepresentation BetActionsRepresentation {get;}
        public BoardRepresentation BoardRepresentation {get;}
        public PlayerRepresentation TopPlayerRepresentation {get;}
        public PlayerRepresentation LeftPlayerRepresentation {get;}
        public PlayerRepresentation RightPlayerRepresentation {get;}

        public GameRepresentation(CommandFactory commandFactory, PlayerGameInfo playerGameInfo)
        {
            CommandFactory = commandFactory;
            PlayerGameInfo = playerGameInfo;
            PlayActionList = PlayerGameInfo?.ValidActionIdList
                ?.Select(id => CommandFactory.ConstructAction(id))
                .Where(a => a.GetActionType() == ActionType.PlayAction)
                .Select(a => (PlayAction)a)
                .ToList();
            BetActionList = PlayerGameInfo?.ValidActionIdList
                ?.Select(id => CommandFactory.ConstructAction(id))
                .Where(a => a.GetActionType() == ActionType.BetAction)
                .Select(a => (BetAction)a)
                .ToList();
            CardCommandDictionary = PlayActionList
                ?.ToDictionary(a => a.Card, a => CommandFactory.ConstructCommand(a.GetActionId()) as ICommand);
            BetActionCommandDictionary = BetActionList
                ?.ToDictionary(a => a, a => CommandFactory.ConstructCommand(a.GetActionId()) as ICommand);
            HandRepresentation = PlayerGameInfo?.PlayerHand != null
                ? new HandRepresentation(PlayerGameInfo?.PlayerHand, CardCommandDictionary)
                : null;
            BetActionsRepresentation = BetActionList != null
                ? new BetActionsRepresentation(BetActionList, BetActionCommandDictionary)
                : null;
            BoardRepresentation = GameStageInfo != null
                ? new BoardRepresentation(PlayerGameInfo.GameInfo, PlayerGameInfo.PlayerInfo, GameStageInfo)
                : null;

            // Remark: Since we receive relative player information we could also directly determine the player positions.
            //         But this way the ViewModel would also work with "real" player informations.
            var topPlayer = PlayerGameInfo.GameInfo.PlayerGroupInfo.GetOppositePlayer(PlayerGameInfo.PlayerInfo.PlayerId);
            var rightPlayer = PlayerGameInfo.GameInfo.PlayerGroupInfo.GetNextPlayer(PlayerGameInfo.PlayerInfo.PlayerId);
            var leftPlayer = PlayerGameInfo.GameInfo.PlayerGroupInfo.GetPreviousPlayer(PlayerGameInfo.PlayerInfo.PlayerId);

            var previousBetActionList = PlayerGameInfo?.GameStageInfo?.ExpectedActionType == ActionType.BetAction
                ? PlayerGameInfo?.GameStageInfo?.CurrentBetActionList
                : null;
            var betActionKeyValuePairList = (previousBetActionList != null && previousBetActionList.Any())
                ? previousBetActionList
                    ?.Select((a,i) => new KeyValuePair<int, BetAction>(i,a))
                    ?.ToList()
                : null;
            var topBetActionDictionary = betActionKeyValuePairList
                ?.Where(p => p.Value.PlayerInfo.PlayerId == topPlayer.PlayerId)
                ?.ToDictionary(p => p.Key, p => p.Value);
            var rightBetActionDictionary = betActionKeyValuePairList
                ?.Where(p => p.Value.PlayerInfo.PlayerId == rightPlayer.PlayerId)
                ?.ToDictionary(p => p.Key, p => p.Value);
            var leftBetActionDictionary = betActionKeyValuePairList
                ?.Where(p => p.Value.PlayerInfo.PlayerId == leftPlayer.PlayerId)
                ?.ToDictionary(p => p.Key, p => p.Value);

            // TODO
            var currentPlayerId = GameStageInfo?.CurrentPlayer?.PlayerId;
            TopPlayerRepresentation = new PlayerRepresentation(topPlayer, topBetActionDictionary, null, 0, currentPlayerId == topPlayer.PlayerId);
            LeftPlayerRepresentation = new PlayerRepresentation(rightPlayer, rightBetActionDictionary, null, 0, currentPlayerId == leftPlayer.PlayerId);
            RightPlayerRepresentation = new PlayerRepresentation(leftPlayer, leftBetActionDictionary, null, 0, currentPlayerId == rightPlayer.PlayerId);
        }
    }
}