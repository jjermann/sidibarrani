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
        private CommandFactory _commandFactory;

        private PlayerGameInfo _playerGameInfo;
        public PlayerGameInfo PlayerGameInfo
        {
            get { return _playerGameInfo; }
            set { this.RaiseAndSetIfChanged(ref _playerGameInfo, value); }
        }

        private ObservableAsPropertyHelper<GameStageInfo> _gameStageInfo;
        private GameStageInfo GameStageInfo
        {
            get { return _gameStageInfo.Value; }
        }

        private ObservableAsPropertyHelper<IList<PlayAction>> _playActionList;
        private IList<PlayAction> PlayActionList
        {
            get { return _playActionList.Value; }
        }

        private ObservableAsPropertyHelper<IList<BetAction>> _betActionList;
        private IList<BetAction> BetActionList
        {
            get { return _betActionList.Value; }
        }

        private ObservableAsPropertyHelper<HandRepresentation> _handRepresentation;
        public HandRepresentation HandRepresentation
        {
            get { return _handRepresentation.Value; }
        }

        private ObservableAsPropertyHelper<BetActionsRepresentation> _betActionsRepresentation;
        public BetActionsRepresentation BetActionsRepresentation
        {
            get { return _betActionsRepresentation.Value; }
        }

        private ObservableAsPropertyHelper<BoardRepresentation> _boardRepresentation;
        public BoardRepresentation BoardRepresentation
        {
            get { return _boardRepresentation.Value; }
        }

        private ObservableAsPropertyHelper<PlayerRepresentation> _topPlayerRepresentation;
        public PlayerRepresentation TopPlayerRepresentation {
            get { return _topPlayerRepresentation.Value; }
        }

        private ObservableAsPropertyHelper<PlayerRepresentation> _leftPlayerRepresentation;
        public PlayerRepresentation LeftPlayerRepresentation {
            get { return _leftPlayerRepresentation.Value; }
        }

        private ObservableAsPropertyHelper<PlayerRepresentation> _rightPlayerRepresentation;
        public PlayerRepresentation RightPlayerRepresentation {
            get { return _rightPlayerRepresentation.Value; }
        }

        public GameRepresentation(CommandFactory commandFactory, PlayerGameInfo playerGameInfo)
        {
            _commandFactory = commandFactory;
            this.WhenAnyValue(
                x => x.PlayerGameInfo,
                x => x.PlayerGameInfo.GameStageInfo,
                (pg, gs) => pg?.GameStageInfo)
                .ToProperty(this, x => x.GameStageInfo, out _gameStageInfo, null);
            this.WhenAnyValue(
                x => x.PlayerGameInfo,
                x => x.PlayerGameInfo.ValidActionList,
                (pg, al) =>
                {
                    var playActionList = pg?.ValidActionList
                        ?.Select(a => _commandFactory.ConstructAction(a.ActionId))
                        .Where(a => a.GetActionType() == ActionType.PlayAction)
                        .Select(a => (PlayAction)a)
                        .ToList();
                    return playActionList;
                })
                .ToProperty(this, x => x.PlayActionList, out _playActionList, null);
            this.WhenAnyValue(
                x => x.PlayerGameInfo,
                x => x.PlayerGameInfo.ValidActionList,
                (pg, al) =>
                {
                    var betActionList = pg?.ValidActionList
                        ?.Select(a => _commandFactory.ConstructAction(a.ActionId))
                        .Where(a => a.GetActionType() == ActionType.BetAction)
                        .Select(a => (BetAction)a)
                        .ToList();
                    return betActionList;
                })
                .ToProperty(this, x => x.BetActionList, out _betActionList, null);
            this.WhenAnyValue(
                x => x.PlayerGameInfo,
                x => x.PlayerGameInfo.PlayerHand,
                x => x.PlayActionList,
                (pg, ph, al) => {
                    var playerHand = pg?.PlayerHand;
                    if (playerHand == null)
                    {
                        return null;
                    }
                    return new HandRepresentation(playerHand, al);
                })
                .ToProperty(this, x => x.HandRepresentation, out _handRepresentation, null);
            this.WhenAnyValue(x => x.BetActionList)
                .Select(x => new BetActionsRepresentation(x))
                .ToProperty(this, x => x.BetActionsRepresentation, out _betActionsRepresentation, null);
            this.WhenAnyValue(x => x.GameStageInfo)
                .Select(x => x != null ? new BoardRepresentation(x) : null)
                .ToProperty(this, x => x.BoardRepresentation, out _boardRepresentation, null);
            var somePlayerInfo = new PlayerInfo();
            this.WhenAnyValue(x => x.GameStageInfo)
                .Select(x => new PlayerRepresentation(somePlayerInfo))
                .ToProperty(this, x => x.TopPlayerRepresentation, out _topPlayerRepresentation, null);
            this.WhenAnyValue(x => x.GameStageInfo)
                .Select(x => new PlayerRepresentation(somePlayerInfo))
                .ToProperty(this, x => x.LeftPlayerRepresentation, out _leftPlayerRepresentation, null);
            this.WhenAnyValue(x => x.GameStageInfo)
                .Select(x => new PlayerRepresentation(somePlayerInfo))
                .ToProperty(this, x => x.RightPlayerRepresentation, out _rightPlayerRepresentation, null);

            PlayerGameInfo = playerGameInfo;
        }
    }
}