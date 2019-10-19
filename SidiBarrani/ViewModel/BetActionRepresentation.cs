using SidiBarraniCommon.Action;
using SidiBarraniCommon.Model;

namespace SidiBarrani.ViewModel
{
    public class BetActionRepresentation
    {
        private BetAction BetAction {get;}
        public BetRepresentation BetRepresentation {get;}
        public int? Index {get;}
        public bool HasIndex => Index.HasValue;
        public bool IsPass => BetAction.Type == BetActionType.Pass;
        public bool IsSidi => BetAction.Type == BetActionType.Sidi;
        public bool IsBarrani => BetAction.Type == BetActionType.Barrani;
        public bool IsCurrentBetAction {get;}

        public BetActionRepresentation(BetAction betAction, int? index = null, bool isCurrentBetAction = false)
        {
            BetAction = betAction;
            BetRepresentation = BetAction.Type == BetActionType.Bet
                ? new BetRepresentation(betAction.Bet)
                : null;
            Index = index;
            IsCurrentBetAction = isCurrentBetAction;
        }
    }
}