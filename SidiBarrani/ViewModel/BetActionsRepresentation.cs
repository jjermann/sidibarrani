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
    public class BetActionsRepresentation : ReactiveObject
    {
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
        private ObservableAsPropertyHelper<IList<BetsRepresentation>> _betsRepresentationList;
        public IList<BetsRepresentation> BetsRepresentationList {
            get { return _betsRepresentationList.Value; }
        }
        private PlayerContext PlayerContext {get;}

        private BetActionsRepresentation() { }
        public BetActionsRepresentation(PlayerContext playerContext)
        {
            PlayerContext = playerContext;
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
                    .Select(g => new BetsRepresentation(g.Key, g.Select(a => a.Bet.BetAmount).ToList()))
                    .ToList())
                .ToProperty(this, x => x.BetsRepresentationList, out _betsRepresentationList);
        }
    }
}