using System;
using System.Collections.Generic;
using System.Linq;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;
using SidiBarraniCommon.Result;

namespace SidiBarraniServer.Game
{
    public class GameRound
    {
        private Rules Rules {get;set;}
        private PlayerGroupInfo PlayerGroupInfo {get;set;}
        private PlayerInfo InitialPlayer {get;set;}
        private IDictionary<string,CardPile> PlayerHandDictionary {get;}

        private BetStage BetStage {get;set;}
        private PlayStage PlayStage {get;set;}

        public BetResult BetResult => BetStage?.BetResult;
        public PlayResult PlayResult => PlayStage?.PlayResult;
        public RoundResult RoundResult {get;set;}
        public ActionType GameRoundActionTypeState {get;set;}

        public GameRound(Rules rules, PlayerGroupInfo playerGroup, PlayerInfo initialPlayer, CardPile deck)
        {
            Rules = rules;
            PlayerGroupInfo = playerGroup;
            InitialPlayer = initialPlayer;
            var playerList = PlayerGroupInfo.GetPlayerList(InitialPlayer.PlayerId);
            PlayerHandDictionary = playerList
                .ToDictionary(p => p.PlayerId, p => new CardPile(deck.Draw(9)));
            BetStage = new BetStage(Rules, PlayerGroupInfo, PlayerHandDictionary, InitialPlayer);
            GameRoundActionTypeState = ActionType.BetAction;
        }

        public IList<int> GetValidActionIdList(string playerId)
        {
            switch (GameRoundActionTypeState)
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
            //TODO: How to handle confirm actions??
            switch (action.GetActionType())
            {
                // case ActionType.SetupAction:
                //     ProcessSetupAction((SetupAction)action);
                //     return;
                case ActionType.BetAction:
                    BetStage.ProcessBetAction((BetAction)action);
                    if (BetStage.BetResult != null)
                    {
                        var playType = BetStage.BetResult.Bet.PlayType;
                        PlayStage = new PlayStage(Rules, PlayerGroupInfo, PlayerHandDictionary, InitialPlayer, playType);
                        GameRoundActionTypeState = ActionType.PlayAction;
                    }
                    return;
                case ActionType.PlayAction:
                    PlayStage.ProcessPlayAction((PlayAction)action);
                    if (PlayStage.PlayResult != null)
                    {
                        RoundResult = GetRoundResult();
                        GameRoundActionTypeState = ActionType.Invalid;
                    }
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

        private RoundResult GetRoundResult()
        {
            var betTeamScore = BetResult.BettingTeam == PlayerGroupInfo.Team1
                ? PlayResult.Team1Score
                : PlayResult.Team2Score;
            var otherTeamScore = BetResult.BettingTeam == PlayerGroupInfo.Team1
                ? PlayResult.Team2Score
                : PlayResult.Team1Score;
            var betSucceded = BetResult.Bet.IsSuccessFor(betTeamScore);
            if (betSucceded)
            {
                var betTeamFinalScore = (betTeamScore.GetRoundedAmount() + BetResult.Bet.BetAmount.GetRoundedAmount()) / 10;
                if (BetResult.IsBarrani)
                {
                    betTeamFinalScore *= 4;
                }
                else if (BetResult.IsSidi)
                {
                    betTeamFinalScore *= 2;
                }
                var otherTeamFinalScore = otherTeamScore.GetRoundedAmount() / 10;
                if (BetResult.IsBarrani)
                {
                    otherTeamFinalScore *= 0;
                }
                else if (BetResult.IsSidi)
                {
                    otherTeamFinalScore *= 2;
                }
                var roundResult = new RoundResult
                {
                    WinningTeam = BetResult.BettingTeam,
                    PlayerGroup = PlayerGroupInfo,
                    Team1FinalScore = BetResult.BettingTeam == PlayerGroupInfo.Team1
                        ? betTeamFinalScore
                        : otherTeamFinalScore,
                    Team2FinalScore = BetResult.BettingTeam == PlayerGroupInfo.Team2
                        ? betTeamFinalScore
                        : otherTeamFinalScore,
                };
                return roundResult;
            }
            else
            {
                var otherTeamFinalScore = (otherTeamScore.GetRoundedAmount() + BetResult.Bet.BetAmount.GetRoundedAmount()) / 10;
                if (BetResult.IsBarrani)
                {
                    otherTeamFinalScore *= 4;
                }
                else if (BetResult.IsSidi)
                {
                    otherTeamFinalScore *= 2;
                }
                var roundResult = new RoundResult
                {
                    WinningTeam = PlayerGroupInfo.GetOtherTeam(BetResult.BettingTeam.TeamId),
                    PlayerGroup = PlayerGroupInfo,
                    Team1FinalScore = BetResult.BettingTeam == PlayerGroupInfo.Team1
                        ? 0
                        : otherTeamFinalScore,
                    Team2FinalScore = BetResult.BettingTeam == PlayerGroupInfo.Team2
                        ? 0
                        : otherTeamFinalScore,
                };
                return roundResult;
            }
        }
    }
}