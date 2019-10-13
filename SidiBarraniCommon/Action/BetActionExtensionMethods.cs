using SidiBarraniCommon.Model;

namespace SidiBarraniCommon.Action
{
    public static class BetActionExtensionMethods
    {
        public static bool IsRegularBetAction(this BetAction betAction)
        {
            return betAction.Type == BetActionType.Bet || betAction.Type == BetActionType.Pass;
        }
    }
}