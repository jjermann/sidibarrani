using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using SidiBarrani.Model;

namespace SidiBarrani.ViewModel
{
    public class TableRepresentation : ReactiveObject
    {
        private ObservableAsPropertyHelper<CardRepresentation> _mainCardRepresentation;
        public CardRepresentation MainCardRepresentation {
            get { return _mainCardRepresentation.Value; }
        }
        private ObservableAsPropertyHelper<CardRepresentation> _leftCardRepresentation;
        public CardRepresentation LeftCardRepresentation {
            get { return _leftCardRepresentation.Value; }
        }
        private ObservableAsPropertyHelper<CardRepresentation> _rightCardRepresentation;
        public CardRepresentation RightCardRepresentation {
            get { return _rightCardRepresentation.Value; }
        }
        private ObservableAsPropertyHelper<CardRepresentation> _oppositeCardRepresentation;
        public CardRepresentation OppositeCardRepresentation {
            get { return _oppositeCardRepresentation.Value; }
        }
        private ObservableAsPropertyHelper<StickRound> _stickRound;
        public StickRound StickRound {
            get { return _stickRound.Value; }
        }
        private Game Game {get;}
        private IList<Player> PlayerList {get;}
        private TableRepresentation() { }
        public TableRepresentation(Game game, IList<Player> playerList)
        {
            Game = game;
            PlayerList = playerList;
            var mainPlayer = PlayerList.First();
            var leftPlayer = PlayerList.Last();
            var rightPlayer = PlayerList.ElementAt(1);
            var oppositePlayer = PlayerList.ElementAt(2);

            this.WhenAnyValue(x => x.Game.GameRound.PlayStage.CurrentStickRound)
                .Select(stickRound => stickRound)
                .ToProperty(this, x => x.StickRound, out _stickRound, null);
            this.WhenAnyValue(x => x.StickRound.PlayActionList)
                .Select(playActionList => playActionList?.SingleOrDefault(playAction => playAction.Player == mainPlayer)?.Card)
                .Select(c => c == null ? null : new CardRepresentation(c))
                .ToProperty(this, x => x.MainCardRepresentation, out _mainCardRepresentation, null);
            this.WhenAnyValue(x => x.StickRound.PlayActionList)
                .Select(playActionList => playActionList?.SingleOrDefault(playAction => playAction.Player == leftPlayer)?.Card)
                .Select(c => c == null ? null : new CardRepresentation(c))
                .ToProperty(this, x => x.LeftCardRepresentation, out _leftCardRepresentation, null);
            this.WhenAnyValue(x => x.StickRound.PlayActionList)
                .Select(playActionList => playActionList?.SingleOrDefault(playAction => playAction.Player == rightPlayer)?.Card)
                .Select(c => c == null ? null : new CardRepresentation(c))
                .ToProperty(this, x => x.RightCardRepresentation, out _rightCardRepresentation, null);
            this.WhenAnyValue(x => x.StickRound.PlayActionList)
                .Select(playActionList => playActionList?.SingleOrDefault(playAction => playAction.Player == oppositePlayer)?.Card)
                .Select(c => c == null ? null : new CardRepresentation(c))
                .ToProperty(this, x => x.OppositeCardRepresentation, out _oppositeCardRepresentation, null);
        }
    }
}