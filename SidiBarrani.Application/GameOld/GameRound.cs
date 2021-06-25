using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using SidiBarraniCommon.ActionOld;
using SidiBarraniCommon.InfoOld;
using SidiBarraniCommon.Model;
using SidiBarraniCommon.Result;

namespace SidiBarraniServer.GameOld
{
    public class GameRound
    {
        private Rules Rules {get;set;}
        private PlayerGroupInfo PlayerGroupInfo {get;set;}
        private Action? ConfirmAction {get;}
        private PlayerInfo InitialPlayer {get;set;}

        // Core hidden information
        public IDictionary<string,CardPile> PlayerHandDictionary {get;}

        public BetStage BetStage {get;set;}
        public PlayStage? PlayStage {get;set;}

        public BetResult? BetResult => BetStage.BetResult;
        public PlayResult? PlayResult => PlayStage?.PlayResult;
        public RoundResult? RoundResult {get;set;}
        public ActionType? ExpectedActionType {get;set;}

        public GameRound(
            Rules rules,
            PlayerGroupInfo playerGroup,
            Action? confirmAction,
            PlayerInfo initialPlayer,
            CardPile deck)
        {
            Rules = rules;
            PlayerGroupInfo = playerGroup;
            ConfirmAction = confirmAction;
            InitialPlayer = initialPlayer;
            var playerList = PlayerGroupInfo.GetPlayerList(InitialPlayer.PlayerId);
            PlayerHandDictionary = playerList
                .ToDictionary(p => p.PlayerId, _ => new CardPile(deck.Draw(9)));
            BetStage = new BetStage(Rules, PlayerGroupInfo, PlayerHandDictionary, InitialPlayer);
            ExpectedActionType = ActionType.BetAction;
        }

        public IList<int> GetValidActionIdList(string playerId)
        {
            switch (ExpectedActionType)
            {
                case ActionType.BetAction:
                    return BetStage.GetValidActionIdList(playerId);
                case ActionType.PlayAction:
                    return PlayStage!.GetValidActionIdList(playerId);
                default:
                    return new List<int>();
            }
        }

        public void ProcessAction(ActionBase action)
        {
            switch (action.GetActionType())
            {
                case ActionType.BetAction:
                    BetStage.ProcessBetAction((BetAction)action);
                    if (BetStage.BetResult != null)
                    {
                        Log.Information(BetStage.BetResult.ToString());
                        ConfirmAction?.Invoke();
                        var playType = BetStage.BetResult.Bet.PlayType;
                        PlayStage = new PlayStage(Rules, PlayerGroupInfo, ConfirmAction, PlayerHandDictionary, InitialPlayer, playType);
                        ExpectedActionType = ActionType.PlayAction;
                    }
                    return;
                case ActionType.PlayAction:
                    PlayStage!.ProcessPlayAction((PlayAction)action);
                    if (PlayStage.PlayResult != null)
                    {
                        Log.Information(PlayStage.PlayResult.ToString());
                        ConfirmAction?.Invoke();
                        RoundResult = GetRoundResult();
                        ExpectedActionType = ActionType.Invalid;
                    }
                    return;
                default:
                    var msg = $"Invalid ActionType {action.GetActionType()}!";
                    Log.Error(msg);
                    throw new Exception(msg);
            }
        }

        private RoundResult GetRoundResult()
        {
            var betTeamScore = BetResult!.BettingTeam == PlayerGroupInfo.Team1
                ? PlayResult!.Team1Score
                : PlayResult!.Team2Score;
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

                var winningTeam = BetResult.BettingTeam;
                var team1FinalScore = BetResult.BettingTeam == PlayerGroupInfo.Team1
                    ? betTeamFinalScore
                    : otherTeamFinalScore;
                var team2FinalScore = BetResult.BettingTeam == PlayerGroupInfo.Team2
                    ? betTeamFinalScore
                    : otherTeamFinalScore;
                var roundResult = new RoundResult(
                    winningTeam,
                    PlayerGroupInfo,
                    team1FinalScore,
                    team2FinalScore);

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

                var winningTeam = PlayerGroupInfo.GetOtherTeam(BetResult.BettingTeam.TeamId);
                var team1FinalScore = BetResult.BettingTeam == PlayerGroupInfo.Team1
                    ? 0
                    : otherTeamFinalScore;
                var team2FinalScore = BetResult.BettingTeam == PlayerGroupInfo.Team2
                    ? 0
                    : otherTeamFinalScore;
                var roundResult = new RoundResult(
                    winningTeam,
                    PlayerGroupInfo,
                    team1FinalScore,
                    team2FinalScore);

                return roundResult;
            }
        }
    }
}