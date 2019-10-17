
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using SidiBarraniCommon.Info;

namespace SidiBarrani.ViewModel
{
    public class PlayerRepresentation : ReactiveObject
    {
        private ObservableAsPropertyHelper<IList<BetRepresentation>> _betRepresentationList;
        public IList<BetRepresentation> BetRepresentationList => _betRepresentationList.Value;

        private ObservableAsPropertyHelper<BetRepresentation> _betRepresentation;
        public BetRepresentation BetRepresentation => _betRepresentation.Value;

        private ObservableAsPropertyHelper<StickPilesRepresentation> _stickPilesRepresentation;
        public StickPilesRepresentation StickPilesRepresentation => _stickPilesRepresentation.Value;

        private ObservableAsPropertyHelper<int> _cardsInHand;
        public int CardsInHand => _cardsInHand.Value;

        private ObservableAsPropertyHelper<bool> _isCurrentPlayer;
        public bool IsCurrentPlayer => _isCurrentPlayer.Value;

        private string _playerName;
        public string PlayerName
        {
            get => _playerName;
            set => this.RaiseAndSetIfChanged(ref _playerName, value);
        }

        public PlayerRepresentation(PlayerInfo playerInfo)
        {
            //TODO
            this.WhenAnyValue(x => x.PlayerName)
                .Select(x => false)
                .ToProperty(this, x => x.IsCurrentPlayer, out _isCurrentPlayer, false);
            this.WhenAnyValue(x => x.PlayerName)
                .Select(x => 1)
                .ToProperty(this, x => x.CardsInHand, out _cardsInHand, 0);
            this.WhenAnyValue(x => x.PlayerName)
                .Select(x => new List<BetRepresentation>())
                .ToProperty(this, x => x.BetRepresentationList, out _betRepresentationList, null);
            this.WhenAnyValue(x => x.BetRepresentationList)
                .Select(x => x?.LastOrDefault())
                .ToProperty(this, x => x.BetRepresentation, out _betRepresentation, null);
            this.WhenAnyValue(x => x.PlayerName)
                .Select(x => new StickPilesRepresentation(null))
                .ToProperty(this, x => x.StickPilesRepresentation, out _stickPilesRepresentation, null);

            PlayerName = playerInfo.PlayerName;
        }
    }
}