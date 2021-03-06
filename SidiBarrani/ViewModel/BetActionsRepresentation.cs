using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Model;

namespace SidiBarrani.ViewModel
{
    public class BetActionsRepresentation : ReactiveObject
    {
        private IList<BetAction> BetActionList {get;}
        public IList<PlayTypeRepresentation> PlayTypeRepresentationList {get;}
        private IDictionary<BetAction, ICommand> BetActionCommandDictionary {get;}
        public ICommand PassBetActionCommand {get;}
        public ICommand SidiBetActionCommand {get;}
        public ICommand BarraniBetActionCommand {get;}
        private ObservableAsPropertyHelper<ICommand> _betBetActionCommand;
        public ICommand BetBetActionCommand => _betBetActionCommand.Value;
        public bool CanAct => BetActionList != null && BetActionList.Any(a => a != null);
        public bool CanBet => BetActionList != null && BetActionList.Any(a => a != null && a.Type == BetActionType.Bet);
        public bool CanPass => BetActionList != null && BetActionList.Any(a => a != null && a.Type == BetActionType.Pass);
        public bool CanSidi => BetActionList != null && BetActionList.Any(a => a != null && a.Type == BetActionType.Sidi);
        public bool CanBarrani => BetActionList != null && BetActionList.Any(a => a != null && a.Type == BetActionType.Barrani);

        private PlayTypeRepresentation _selectedPlayTypeRepresentation;
        public PlayTypeRepresentation SelectedPlayTypeRepresentation
        {
            get => _selectedPlayTypeRepresentation;
            set => this.RaiseAndSetIfChanged(ref _selectedPlayTypeRepresentation, value);
        }
        private ScoreAmountRepresentation _selectedScoreAmountRepresentation;
        public ScoreAmountRepresentation SelectedScoreAmountRepresentation
        {
            get => _selectedScoreAmountRepresentation;
            set => this.RaiseAndSetIfChanged(ref _selectedScoreAmountRepresentation, value);
        }
        private ObservableAsPropertyHelper<IList<ScoreAmountRepresentation>> _scoreAmountRepresentationList;
        public IList<ScoreAmountRepresentation> ScoreAmountRepresentationList => _scoreAmountRepresentationList.Value;
        private ObservableAsPropertyHelper<BetRepresentation> _selectedBetRepresentation;
        public BetRepresentation SelectedBetRepresentation => _selectedBetRepresentation.Value;
        private ObservableAsPropertyHelper<bool> _hasBetSelected;
        public bool HasBetSelected => _hasBetSelected.Value;

        public BetActionsRepresentation(IList<BetAction> betActionList, IDictionary<BetAction, ICommand> commandDictionary)
        {
            BetActionList = betActionList;
            PlayTypeRepresentationList = BetActionList
                ?.Where(a => a != null)
                .Where(a => a.Type == BetActionType.Bet)
                .GroupBy(a => a.Bet.PlayType)
                .Select(g => new PlayTypeRepresentation(g.Key, 30.0))
                .ToList();
            BetActionCommandDictionary = commandDictionary;
            PassBetActionCommand = commandDictionary?.SingleOrDefault(p => p.Key.Type == BetActionType.Pass).Value;
            SidiBetActionCommand = commandDictionary?.SingleOrDefault(p => p.Key.Type == BetActionType.Sidi).Value;
            BarraniBetActionCommand = commandDictionary?.SingleOrDefault(p => p.Key.Type == BetActionType.Barrani).Value;
            this.WhenAnyValue(x => x.SelectedPlayTypeRepresentation)
                .Select(r => BetActionList
                    ?.Where(a => a != null && r != null)
                    .Where(a => a.Type == BetActionType.Bet && a.Bet.PlayType == r.PlayType)
                    .Select(a => new ScoreAmountRepresentation(a.Bet.BetAmount))
                    .ToList())
                .ToProperty(this, x => x.ScoreAmountRepresentationList, out _scoreAmountRepresentationList);
            this.WhenAnyValue(
                x => x.SelectedPlayTypeRepresentation,
                x => x.SelectedScoreAmountRepresentation,
                (p,s) => BetActionList?.SingleOrDefault(a => (a?.Bet != null && p!= null && s != null) && a.Bet.PlayType == p.PlayType && a.Bet.BetAmount == s.ScoreAmount))
                .Select(a => a == null ? null : new BetRepresentation(a.Bet))
                .ToProperty(this, x => x.SelectedBetRepresentation, out _selectedBetRepresentation);
            this.WhenAnyValue(x => x.SelectedBetRepresentation)
                .Select(r => BetActionCommandDictionary
                    ?.Where(p => r != null && p.Key.Type == BetActionType.Bet && p.Key.Bet == r.Bet)
                    ?.Select(p => p.Value)
                    ?.SingleOrDefault())
                .ToProperty(this, x => x.BetBetActionCommand, out _betBetActionCommand);
            this.WhenAnyValue(x => x.SelectedBetRepresentation)
                .Select(r => r != null)
                .ToProperty(this, x => x.HasBetSelected, out _hasBetSelected);
        }
    }
}