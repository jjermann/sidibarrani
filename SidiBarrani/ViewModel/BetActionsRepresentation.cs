using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using SidiBarrani.Model;

namespace SidiBarrani.ViewModel
{
    public class BetActionsRepresentation : ReactiveObject
    {
        private PlayerContext PlayerContext {get;}
        private ObservableAsPropertyHelper<IReadOnlyCollection<BetAction>> _betActionList;
        private IReadOnlyCollection<BetAction> BetActionList {
            get { return _betActionList.Value; }
        }
        private ObservableAsPropertyHelper<bool> _canAct;
        public bool CanAct {
            get { return _canAct.Value; }
        }
        private ObservableAsPropertyHelper<bool> _canBet;
        public bool CanBet {
            get { return _canBet.Value; }
        }
        private ObservableAsPropertyHelper<bool> _canPass;
        public bool CanPass {
            get { return _canPass.Value; }
        }
        private ObservableAsPropertyHelper<bool> _canSidi;
        public bool CanSidi {
            get { return _canSidi.Value; }
        }
        private ObservableAsPropertyHelper<bool> _canBarrani;
        public bool CanBarrani {
            get { return _canAct.Value; }
        }
        private ObservableAsPropertyHelper<IList<PlayTypeRepresentation>> _playTypeRepresentationList;
        public IList<PlayTypeRepresentation> PlayTypeRepresentationList {
            get { return _playTypeRepresentationList.Value; }
        }
        private PlayTypeRepresentation _selectedPlayTypeRepresentation;
        public PlayTypeRepresentation SelectedPlayTypeRepresentation
        {
            get { return _selectedPlayTypeRepresentation; }
            set { this.RaiseAndSetIfChanged(ref _selectedPlayTypeRepresentation, value); }
        }
        private ObservableAsPropertyHelper<IList<ScoreAmountRepresentation>> _scoreAmountRepresentationList;
        public IList<ScoreAmountRepresentation> ScoreAmountRepresentationList {
            get { return _scoreAmountRepresentationList.Value; }
        }
        private ScoreAmountRepresentation _selectedScoreAmountRepresentation;
        public ScoreAmountRepresentation SelectedScoreAmountRepresentation
        {
            get { return _selectedScoreAmountRepresentation; }
            set { this.RaiseAndSetIfChanged(ref _selectedScoreAmountRepresentation, value); }
        }
        private ObservableAsPropertyHelper<BetRepresentation> _selectedBetRepresentation;
        public BetRepresentation SelectedBetRepresentation {
            get { return _selectedBetRepresentation.Value; }
        }
        private ObservableAsPropertyHelper<bool> _hasBetSelected;
        public bool HasBetSelected {
            get { return _hasBetSelected.Value; }
        }
        public ReactiveCommand<string, BetAction> BetActionCommand {get;}

        private BetActionsRepresentation() { }
        public BetActionsRepresentation(PlayerContext playerContext)
        {
            PlayerContext = playerContext;
            BetActionCommand = ReactiveCommand.Create<string, BetAction>(t =>
            {
                switch(t)
                {
                    case "Pass":
                        return BetActionList.SingleOrDefault(a => a != null && a.Type == BetActionType.Pass);
                    case "Sidi":
                        return BetActionList.SingleOrDefault(a => a != null && a.Type == BetActionType.Sidi);
                    case "Barrani":
                        return BetActionList.SingleOrDefault(a => a != null && a.Type == BetActionType.Barrani);
                    case "Bet":
                        return BetActionList.SingleOrDefault(a => a != null && SelectedBetRepresentation?.Bet != null && a.Type == BetActionType.Bet && a.Bet == SelectedBetRepresentation.Bet);
                    default:
                        throw new ArgumentException();
                }
            });
            PlayerContext.AvailableBetActions
                .Connect()
                .ToCollection()
                .ToProperty(this, x => x.BetActionList, out _betActionList, new ReadOnlyCollection<BetAction>(new List<BetAction>()));
            this.WhenAnyValue(x => x.BetActionList)
                .Select(betActionList => betActionList != null && betActionList.Any(a => a != null))
                .ToProperty(this, x => x.CanAct, out _canAct);
            this.WhenAnyValue(x => x.BetActionList)
                .Select(betActionList => betActionList != null && betActionList.Any(a => a != null && a.Type == BetActionType.Bet))
                .ToProperty(this, x => x.CanBet, out _canBet);
            this.WhenAnyValue(x => x.BetActionList)
                .Select(betActionList => betActionList != null && betActionList.Any(a => a != null && a.Type == BetActionType.Pass))
                .ToProperty(this, x => x.CanPass, out _canPass);
            this.WhenAnyValue(x => x.BetActionList)
                .Select(betActionList => betActionList != null && betActionList.Any(a => a != null && a.Type == BetActionType.Sidi))
                .ToProperty(this, x => x.CanSidi, out _canSidi);
            this.WhenAnyValue(x => x.BetActionList)
                .Select(betActionList => betActionList != null && betActionList.Any(a => a != null && a.Type == BetActionType.Barrani))
                .ToProperty(this, x => x.CanBarrani, out _canBarrani);
            this.WhenAnyValue(x => x.BetActionList)
                .Select(betActionList => betActionList
                    ?.Where(a => a != null)
                    .Where(a => a.Type == BetActionType.Bet)
                    .GroupBy(a => a.Bet.PlayType)
                    .Select(g => new PlayTypeRepresentation(g.Key, 30.0))
                    .ToList())
                .ToProperty(this, x => x.PlayTypeRepresentationList, out _playTypeRepresentationList);
            this.WhenAnyValue(
                x => x.BetActionList,
                x => x.SelectedPlayTypeRepresentation,
                (betActionList,r) => betActionList
                    ?.Where(a => a != null && r != null)
                    .Where(a => a.Type == BetActionType.Bet && a.Bet.PlayType == r.PlayType)
                    .Select(a => new ScoreAmountRepresentation(a.Bet.BetAmount))
                    .ToList())
                .ToProperty(this, x => x.ScoreAmountRepresentationList, out _scoreAmountRepresentationList);
            this.WhenAnyValue<BetActionsRepresentation, BetAction, IReadOnlyCollection<BetAction>, PlayTypeRepresentation, ScoreAmountRepresentation>(
                x => x.BetActionList,
                x => x.SelectedPlayTypeRepresentation,
                x => x.SelectedScoreAmountRepresentation,
                (l,p,s) => l?.SingleOrDefault(a => (a?.Bet != null && p!= null && s != null) && a.Bet.PlayType == p.PlayType && a.Bet.BetAmount == s.ScoreAmount))
                .Select(a => a == null ? null : new BetRepresentation(a.Bet))
                .ToProperty(this, x => x.SelectedBetRepresentation, out _selectedBetRepresentation);
            this.WhenAnyValue(x => x.SelectedBetRepresentation)
                .Select(r => r != null)
                .ToProperty(this, x => x.HasBetSelected, out _hasBetSelected);
        }
    }
}