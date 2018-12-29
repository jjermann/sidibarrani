using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
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
        private ObservableAsPropertyHelper<IReadOnlyCollection<CardRepresentation>> _mainCardsInHand;
        public IReadOnlyCollection<CardRepresentation> MainCardsInHand {
            get { return _mainCardsInHand.Value; }
        }
        private ObservableAsPropertyHelper<IReadOnlyCollection<CardRepresentation>> _leftCardsInHand;
        public IReadOnlyCollection<CardRepresentation> LeftCardsInHand {
            get { return _leftCardsInHand.Value; }
        }
        private ObservableAsPropertyHelper<IReadOnlyCollection<CardRepresentation>> _rightCardsInHand;
        public IReadOnlyCollection<CardRepresentation> RightCardsInHand {
            get { return _rightCardsInHand.Value; }
        }
        private ObservableAsPropertyHelper<IReadOnlyCollection<CardRepresentation>> _oppositeCardsInHand;
        public IReadOnlyCollection<CardRepresentation> OppositeCardsInHand {
            get { return _oppositeCardsInHand.Value; }
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
            mainPlayer.Context.CardsInHand
                .Connect()
                .Transform(c => new CardRepresentation(c))
                .ToCollection()
                .ToProperty(this, x => x.MainCardsInHand, out _mainCardsInHand, new ReadOnlyCollection<CardRepresentation>(new List<CardRepresentation>()));
            leftPlayer.Context.CardsInHand
                .Connect()
                .Transform(c => new CardRepresentation(c) { IsFaceUp = false })
                .ToCollection()
                .ToProperty(this, x => x.LeftCardsInHand, out _leftCardsInHand, new ReadOnlyCollection<CardRepresentation>(new List<CardRepresentation>()));
            rightPlayer.Context.CardsInHand
                .Connect()
                .Transform(c => new CardRepresentation(c) { IsFaceUp = false })
                .ToCollection()
                .ToProperty(this, x => x.RightCardsInHand, out _rightCardsInHand, new ReadOnlyCollection<CardRepresentation>(new List<CardRepresentation>()));
            oppositePlayer.Context.CardsInHand
                .Connect()
                .Transform(c => new CardRepresentation(c) { IsFaceUp = false })
                .ToCollection()
                .ToProperty(this, x => x.OppositeCardsInHand, out _oppositeCardsInHand, new ReadOnlyCollection<CardRepresentation>(new List<CardRepresentation>()));
        }
    }
}