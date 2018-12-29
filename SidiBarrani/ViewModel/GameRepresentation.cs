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
    public class GameRepresentation : ReactiveObject
    {
        public MainBoardRepresentation MainBoardRepresentation {get;}
        public HandRepresentation MainHandRepresentation {get;}
        public HandRepresentation RightHandRepresentation {get;}
        public HandRepresentation OppositeHandRepresentation {get;}
        public HandRepresentation LeftHandRepresentation {get;}

        private Game Game {get;}
        private IList<Player> PlayerList {get;}
        private GameRepresentation() { }
        public GameRepresentation(Game game, IList<Player> playerList)
        {
            Game = game;
            PlayerList = playerList;
            var mainPlayer = PlayerList.First();
            var leftPlayer = PlayerList.Last();
            var rightPlayer = PlayerList.ElementAt(1);
            var oppositePlayer = PlayerList.ElementAt(2);
            MainBoardRepresentation = new MainBoardRepresentation(Game, PlayerList);
            MainHandRepresentation = new HandRepresentation(mainPlayer.Context);
            RightHandRepresentation = new HandRepresentation(rightPlayer.Context);
            OppositeHandRepresentation = new HandRepresentation(oppositePlayer.Context);
            LeftHandRepresentation = new HandRepresentation(leftPlayer.Context);
        }
    }
}