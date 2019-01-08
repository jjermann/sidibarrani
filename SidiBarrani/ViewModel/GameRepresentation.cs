using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
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

        private ObservableAsPropertyHelper<BetResultRepresentation> _betResultRepresentation;
        public BetResultRepresentation BetResultRepresentation
        {
            get { return _betResultRepresentation.Value; }
        }
        public ReactiveCommand<Unit, Unit> ConfirmCommand {get;}
        private ObservableAsPropertyHelper<bool> _canConfirm;
        public bool CanConfirm
        {
            get { return _canConfirm.Value; }
        }
        public IObservable<BetAction> BetActionObservable {get;}
        public IObservable<PlayAction> PlayActionObservable {get;}

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

            ConfirmCommand = ReactiveCommand.Create(() =>
            {
                mainPlayer.Context.CanConfirm = false;
            });
            BetActionObservable = MainBetActionsRepresentation.BetActionCommand;
            PlayActionObservable = MainHandRepresentation.PlayActionCommand;
            mainPlayer.Context.WhenAnyValue(x => x.CanConfirm)
                .ToProperty(this, x => x.CanConfirm, out _canConfirm, false);
            this.WhenAnyValue(x => x.Game.GameRound.BetResult)
                .Select(betResult => betResult == null ? null : new BetResultRepresentation(betResult))
                .ToProperty(this, x => x.BetResultRepresentation, out _betResultRepresentation, null);
        }
    }
}