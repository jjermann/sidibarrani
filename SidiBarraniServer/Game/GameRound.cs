using System;
using System.Collections.Generic;
using SidiBarraniCommon;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Result;

namespace SidiBarraniServer.Game
{
    public class GameRound
    {
        private BetStage BetStage {get;set;}
        private PlayStage PlayStage {get;set;}

        public PlayerInfo FirstPlayer {get;set;}
        public BetResult BetResult => BetStage?.BetResult;
        public PlayResult PlayResult => PlayStage?.PlayResult;
        public RoundResult RoundResult {get;set;}
        public ActionType GameStateType {get;set;}
        public bool IsActive {get;set;}

        public bool StartRound(PlayerInfo firstPlayer)
        {
            if (IsActive)
            {
                return false;
            }
            BetStage = new BetStage();
            GameStateType = ActionType.BetAction;
            IsActive = true;
            return true;
        }

        public IList<int> GetValidActionIdList(string playerId)
        {
            switch (GameStateType)
            {
                case ActionType.ConfirmAction:
                    return new List<int>
                    {
                        ConfirmAction.GetStaticActionId()
                    };
                case ActionType.BetAction:
                    return BetStage.GetValidActionIdList(playerId);
                case ActionType.PlayAction:
                    return PlayStage.GetValidActionIdList(playerId);
                default:
                    return new List<int>();
            }
        }

        public void ProcessAction(ActionBase action)
        {
            switch (action.GetActionType())
            {
                // case ActionType.SetupAction:
                //     ProcessSetupAction((SetupAction)action);
                //     return;
                case ActionType.BetAction:
                    BetStage.ProcessBetAction((BetAction)action);
                    return;
                case ActionType.PlayAction:
                    PlayStage.ProcessPlayAction((PlayAction)action);
                    return;
                // case ActionType.ConfirmAction:
                //     ProcessConfirmAction((ConfirmAction)action);
                //     return;
                default:
                    throw new Exception($"Invalid ActionType {action?.GetActionType()}!");
            }
        }

        // private bool ProcessSetupAction(SetupAction setupAction)
        // {
        //     return true;
        // }
        // private bool ProcessConfirmAction(ConfirmAction confirmAction)
        // {
        //     return true;
        // }
    }
}