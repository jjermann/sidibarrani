using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using SidiBarrani.Model;

namespace SidiBarrani.ViewModel
{
    public class GameRepresentation : ReactiveObject
    {
        private Game Game {get;}
        private IList<Player> PlayerList {get;}

        public BoardRepresentation BoardRepresentation {get;}
        public BetActionsRepresentation MainBetActionsRepresentation {get;}
        public HandRepresentation MainHandRepresentation {get;}
        public HandRepresentation RightHandRepresentation {get;}
        public HandRepresentation OppositeHandRepresentation {get;}
        public HandRepresentation LeftHandRepresentation {get;}

        private ObservableAsPropertyHelper<PlayTypeRepresentation> _currentPlayTypeRepresentation;
        public PlayTypeRepresentation CurrentPlayTypeRepresentation
        {
            get { return _currentPlayTypeRepresentation.Value; }
        }

        private GameRepresentation() { }
        public GameRepresentation(Game game, IList<Player> playerList)
        {
            Game = game;
            PlayerList = playerList;
            var mainPlayer = PlayerList.First();
            var leftPlayer = PlayerList.Last();
            var rightPlayer = PlayerList.ElementAt(1);
            var oppositePlayer = PlayerList.ElementAt(2);
            BoardRepresentation = new BoardRepresentation(Game, PlayerList);
            MainBetActionsRepresentation = new BetActionsRepresentation(mainPlayer.Context);
            MainHandRepresentation = new HandRepresentation(mainPlayer.Context);
            RightHandRepresentation = new HandRepresentation(rightPlayer.Context);
            OppositeHandRepresentation = new HandRepresentation(oppositePlayer.Context);
            LeftHandRepresentation = new HandRepresentation(leftPlayer.Context);

            this.WhenAnyValue(x => x.Game.GameRound.BetResult)
                .Select(betResult => betResult?.Bet?.PlayType == null ? null : new PlayTypeRepresentation(betResult.Bet.PlayType, 30))
                .ToProperty(this, x => x.CurrentPlayTypeRepresentation, out _currentPlayTypeRepresentation, null);
        }
    }
}