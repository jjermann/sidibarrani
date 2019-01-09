using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace SidiBarrani.Model
{
    public class GameRound : ReactiveObject
    {
        private Rules Rules {get;}
        private PlayerGroup PlayerGroup {get;}
        private Player InitialPlayer {get;}

        private BetStage _betStage;
        public BetStage BetStage
        {
            get { return _betStage; }
            private set { this.RaiseAndSetIfChanged(ref _betStage, value); }
        }
        private ObservableAsPropertyHelper<BetAction> _betAction;
        public BetAction BetAction
        {
            get { return _betAction.Value; }
        }
        private BetResult _betResult;
        public BetResult BetResult
        {
            get { return _betResult; }
            private set { this.RaiseAndSetIfChanged(ref _betResult, value); }
        }
        private PlayStage _playStage;
        public PlayStage PlayStage
        {
            get { return _playStage; }
            private set { this.RaiseAndSetIfChanged(ref _playStage, value); }
        }
        private ObservableAsPropertyHelper<PlayAction> _playAction;
        public PlayAction PlayAction
        {
            get { return _playAction.Value; }
        }
        private PlayResult _playResult;
        public PlayResult PlayResult
        {
            get { return _playResult; }
            private set { this.RaiseAndSetIfChanged(ref _playResult, value); }
        }

        public GameRound(Rules rules, PlayerGroup playerGroup, Player initialPlayer) {
            Rules = rules;
            PlayerGroup = playerGroup;
            InitialPlayer = initialPlayer;
            this.WhenAnyValue(x => x.BetStage.BetActionList)
                .Select(actionList => actionList?.LastOrDefault())
                .ToProperty(this, x => x.BetAction, out _betAction, null);
            this.WhenAnyValue(x => x.PlayStage.CurrentStickRound.PlayActionList)
                .Select(actionList => actionList?.LastOrDefault())
                .ToProperty(this, x => x.PlayAction, out _playAction, null);
        }
        public async Task<RoundResult> ProcessRound()
        {
            var cardPile = CardPile.CreateFullCardPile();
            DistributeCards(PlayerGroup, InitialPlayer, cardPile);
            BetStage = new BetStage(Rules, PlayerGroup, InitialPlayer);
            BetResult = await ProcessBetting(BetStage, PlayerGroup);
            await PlayerGroup.RequestConfirmCommand.Execute();
            BetStage = null;
            PlayStage = new PlayStage(Rules, PlayerGroup, InitialPlayer, BetResult.Bet.PlayType);
            PlayResult = await ProcessPlaying(PlayStage, PlayerGroup);
            await PlayerGroup.RequestConfirmCommand.Execute();
            PlayStage = null;
            var roundResult = DetermineRoundResult(Rules, PlayerGroup, BetResult, PlayResult);
            BetResult = null;
            PlayResult = null;
            return roundResult;
        }

        private static void DistributeCards(PlayerGroup playerGroup, Player initialPlayer, CardPile cardPile)
        {
            var contextDictionary = new Dictionary<Player, PlayerContext>();
            var playerOrder = playerGroup.GetPlayerListFromInitialPlayer(initialPlayer);
            foreach (var player in playerOrder)
            {
                player.Context.CardsInHand.Clear();
                player.Context.AvailableBetActions.Clear();
                player.Context.AvailablePlayActions.Clear();
                player.Context.WonSticks.Clear();
                player.Context.IsCurrentPlayer = player == initialPlayer;
                player.Context.CardsInHand.AddRange(cardPile.Draw(9));
            }
        }

        private async Task<BetResult> ProcessBetting(BetStage betStage, PlayerGroup playerGroup)
        {
            var betResult = betStage.GetBetResult();
            while (betResult == null) {
                var playerList = playerGroup.GetPlayerListFromInitialPlayer(betStage.CurrentPlayer);
                var betAction = await PlayerGroup.RequestBetCommand.Execute(betStage);
                betStage.AddBetActionAndProceed(betAction);
                betResult = betStage.GetBetResult();
            }
            return betResult;
        }

        private async Task<PlayResult> ProcessPlaying(PlayStage playStage, PlayerGroup playerGroup)
        {
            var playResult = playStage.GetPlayResult();
            while (playResult == null)
            {
                var playerList = playerGroup.GetPlayerListFromInitialPlayer(playStage.CurrentPlayer);
                var playAction = await PlayerGroup.RequestPlayCommand.Execute(playStage);
                var stickResult = playStage.AddPlayActionAndProceed(playAction);
                if (stickResult != null)
                {
                    playStage.ProcessStickResult(stickResult);
                    await PlayerGroup.RequestConfirmCommand.Execute();
                    playStage.UpdateStickRound(stickResult);
                }
                playResult = playStage.GetPlayResult();
            }
            return playResult;
        }

        private static RoundResult DetermineRoundResult(Rules rules, PlayerGroup playerGroup, BetResult betResult, PlayResult playResult)
        {
            var betTeamScore = betResult.BettingTeam == playerGroup.Team1
                ? playResult.Team1Score
                : playResult.Team2Score;
            var otherTeamScore = betResult.BettingTeam == playerGroup.Team1
                ? playResult.Team2Score
                : playResult.Team1Score;
            var betSucceded = betResult.Bet.IsSuccessFor(betTeamScore);
            if (betSucceded)
            {
                var betTeamFinalScore = (betTeamScore.GetRoundedAmount() + betResult.Bet.BetAmount.GetRoundedAmount()) / 10;
                if (betResult.IsBarrani)
                {
                    betTeamFinalScore *= 4;
                }
                else if (betResult.IsSidi)
                {
                    betTeamFinalScore *= 2;
                }
                var otherTeamFinalScore = otherTeamScore.GetRoundedAmount() / 10;
                if (betResult.IsBarrani)
                {
                    otherTeamFinalScore *= 0;
                }
                else if (betResult.IsSidi)
                {
                    otherTeamFinalScore *= 2;
                }
                var roundResult = new RoundResult
                {
                    WinningTeam = betResult.BettingTeam,
                    PlayerGroup = playerGroup,
                    Team1FinalScore = betResult.BettingTeam == playerGroup.Team1
                        ? betTeamFinalScore
                        : otherTeamFinalScore,
                    Team2FinalScore = betResult.BettingTeam == playerGroup.Team2
                        ? betTeamFinalScore
                        : otherTeamFinalScore,
                };
                return roundResult;
            }
            else {
                var otherTeamFinalScore = (otherTeamScore.GetRoundedAmount() + betResult.Bet.BetAmount.GetRoundedAmount())/ 10;
                if (betResult.IsBarrani)
                {
                    otherTeamFinalScore *= 4;
                }
                else if (betResult.IsSidi)
                {
                    otherTeamFinalScore *= 2;
                }
                var roundResult = new RoundResult
                {
                    WinningTeam = playerGroup.GetOtherTeam(betResult.BettingTeam),
                    PlayerGroup = playerGroup,
                    Team1FinalScore = betResult.BettingTeam == playerGroup.Team1
                        ? 0
                        : otherTeamFinalScore,
                    Team2FinalScore = betResult.BettingTeam == playerGroup.Team2
                        ? 0
                        : otherTeamFinalScore,
                };
                return roundResult;
            }
        }
    }
}