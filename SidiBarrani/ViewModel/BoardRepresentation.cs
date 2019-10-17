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
            get => _gameStageInfo;
            set => this.RaiseAndSetIfChanged(ref _gameStageInfo, value);
        }

        private ObservableAsPropertyHelper<CardRepresentation> _topCardRepresentation;
        public CardRepresentation TopCardRepresentation => _topCardRepresentation.Value;

        private ObservableAsPropertyHelper<CardRepresentation> _leftCardRepresentation;
        public CardRepresentation LeftCardRepresentation => _leftCardRepresentation.Value;

        private ObservableAsPropertyHelper<CardRepresentation> _rightCardRepresentation;
        public CardRepresentation RightCardRepresentation => _rightCardRepresentation.Value;

        private ObservableAsPropertyHelper<CardRepresentation> _bottomCardRepresentation;
        public CardRepresentation BottomCardRepresentation => _bottomCardRepresentation.Value;

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