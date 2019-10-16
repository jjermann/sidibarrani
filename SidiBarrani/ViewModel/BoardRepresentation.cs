using System.Reactive.Linq;
using ReactiveUI;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;

namespace SidiBarrani.ViewModel
{
    public class BoardRepresentation : ReactiveObject
    {
        private GameStageInfo _gameStageInfo;
        public GameStageInfo GameStageInfo
        {
            get { return _gameStageInfo; }
            set { this.RaiseAndSetIfChanged(ref _gameStageInfo, value); }
        }

        private ObservableAsPropertyHelper<CardRepresentation> _topCardRepresentation;
        public CardRepresentation TopCardRepresentation {
            get { return _topCardRepresentation.Value; }
        }

        private ObservableAsPropertyHelper<CardRepresentation> _leftCardRepresentation;
        public CardRepresentation LeftCardRepresentation {
            get { return _leftCardRepresentation.Value; }
        }

        private ObservableAsPropertyHelper<CardRepresentation> _rightCardRepresentation;
        public CardRepresentation RightCardRepresentation {
            get { return _rightCardRepresentation.Value; }
        }

        private ObservableAsPropertyHelper<CardRepresentation> _bottomCardRepresentation;
        public CardRepresentation BottomCardRepresentation {
            get { return _bottomCardRepresentation.Value; }
        }

        public BoardRepresentation(GameStageInfo gameStageInfo)
        {
            //TODO
            var someCard = new Card();
            this.WhenAnyValue(x => x.GameStageInfo)
                .Select(x => new CardRepresentation(someCard))
                .ToProperty(this, x => x.TopCardRepresentation, out _topCardRepresentation, null);
            this.WhenAnyValue(x => x.GameStageInfo)
                .Select(x => new CardRepresentation(someCard))
                .ToProperty(this, x => x.LeftCardRepresentation, out _leftCardRepresentation, null);
            this.WhenAnyValue(x => x.GameStageInfo)
                .Select(x => new CardRepresentation(someCard))
                .ToProperty(this, x => x.RightCardRepresentation, out _rightCardRepresentation, null);
            this.WhenAnyValue(x => x.GameStageInfo)
                .Select(x => new CardRepresentation(someCard))
                .ToProperty(this, x => x.BottomCardRepresentation, out _bottomCardRepresentation, null);

            GameStageInfo = gameStageInfo;
        }
    }
}