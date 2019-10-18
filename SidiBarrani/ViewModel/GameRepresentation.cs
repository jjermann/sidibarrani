using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Info;

namespace SidiBarrani.ViewModel
{
    public class GameRepresentation : ReactiveObject
    {
        private CommandFactory CommandFactory {get;}
        private PlayerGameInfo PlayerGameInfo {get;}
        private IList<PlayAction> PlayActionList {get;}
        private IList<BetAction> BetActionList {get;}
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
            PlayActionList = PlayerGameInfo?.ValidActionList
                ?.Select(a => CommandFactory.ConstructAction(a.ActionId))
                .Where(a => a.GetActionType() == ActionType.PlayAction)
                .Select(a => (PlayAction)a)
                .ToList();
            BetActionList = PlayerGameInfo?.ValidActionList
                ?.Select(a => CommandFactory.ConstructAction(a.ActionId))
                .Where(a => a.GetActionType() == ActionType.BetAction)
                .Select(a => (BetAction)a)
                .ToList();
            HandRepresentation = PlayerGameInfo?.PlayerHand != null
                ? new HandRepresentation(PlayerGameInfo?.PlayerHand, PlayActionList)
                : null;
            BetActionsRepresentation = BetActionList != null
                ? new BetActionsRepresentation(BetActionList)
                : null;
            BoardRepresentation = GameStageInfo != null
                ? new BoardRepresentation(GameStageInfo)
                : null;
            //TODO
            var somePlayerInfo = new PlayerInfo();
            TopPlayerRepresentation = new PlayerRepresentation(somePlayerInfo);
            LeftPlayerRepresentation = new PlayerRepresentation(somePlayerInfo);
            RightPlayerRepresentation = new PlayerRepresentation(somePlayerInfo);
        }
    }
}