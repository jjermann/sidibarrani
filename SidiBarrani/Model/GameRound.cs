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
        private BetAction _betAction;
        public BetAction BetAction
        {
            get { return _betAction; }
            private set { this.RaiseAndSetIfChanged(ref _betAction, value); }
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
        private PlayAction _playAction;
        public PlayAction PlayAction
        {
            get { return _playAction; }
            private set { this.RaiseAndSetIfChanged(ref _playAction, value); }
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
        }
        public async Task<RoundResult> ProcessRound()
        {
            var cardPile = CardPile.CreateFullCardPile();
            var contextDictionary = DistributeCards(PlayerGroup, InitialPlayer, cardPile);
            foreach (var player in contextDictionary.Keys)
            {
                player.Context = contextDictionary[player];
            }
            BetStage = new BetStage(Rules, PlayerGroup, InitialPlayer);
            BetResult = await ProcessBetting(BetStage, PlayerGroup);
            BetStage = null;
            PlayStage = new PlayStage(Rules, PlayerGroup, InitialPlayer, BetResult.Bet.PlayType);
            PlayResult = await ProcessPlaying(PlayStage, PlayerGroup);
            PlayStage = null;
            var roundResult = DetermineRoundResult(Rules, PlayerGroup, BetResult, PlayResult);
            return roundResult;
        }

        private static IDictionary<Player, PlayerContext> DistributeCards(PlayerGroup playerGroup, Player initialPlayer, CardPile cardPile)
        {
            var contextDictionary = new Dictionary<Player, PlayerContext>();
            var playerOrder = playerGroup.GetPlayerListFromInitialPlayer(initialPlayer);
            foreach (var player in playerOrder)
            {
                var playerContext = new PlayerContext
                {
                    CardsInHand = cardPile.Draw(9)
                };
                contextDictionary[player] = playerContext;
            }
            return contextDictionary;
        }

        private async Task<BetResult> ProcessBetting(BetStage betStage, PlayerGroup playerGroup)
        {
            var betResult = betStage.GetBetResult();
            while (betResult == null) {
                var playerList = playerGroup.GetPlayerListFromInitialPlayer(betStage.CurrentPlayer);
                UpdatePlayerContext(playerList, betStage);
                var betAction = await GetNextBetAction(playerList);
                BetAction = betAction;
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
                UpdatePlayerContext(playerList, playStage);
                var playAction = await GetNextPlayAction(playerList);
                PlayAction = playAction;
                playStage.AddPlayActionAndProceed(playAction);
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

        private static void UpdatePlayerContext(IList<Player> playerList, BetStage betStage)
        {
            var validActionDictionary = betStage
                .GetValidBetActions()
                .GroupBy(a => a.Player)
                .ToDictionary(g => g.Key, g => g.ToList());
            foreach (var player in playerList) {
                var hasActions = validActionDictionary.ContainsKey(player);
                player.Context.AvailableBetActions = hasActions
                    ? validActionDictionary[player]
                    : new List<BetAction>();
                player.Context.IsCurrentPlayer = player == betStage.CurrentPlayer;
            }
        }

        private static void UpdatePlayerContext(IList<Player> playerList, PlayStage playStage)
        {
            var validActionDictionary = playStage
                .GetValidPlayActions()
                .GroupBy(a => a.Player)
                .ToDictionary(g => g.Key, g => g.ToList());
            foreach (var player in playerList) {
                var hasActions = validActionDictionary.ContainsKey(player);
                player.Context.AvailablePlayActions = hasActions
                    ? validActionDictionary[player]
                    : new List<PlayAction>();
                player.Context.IsCurrentPlayer = player == playStage.CurrentPlayer;
            }
        }

        private static async Task<BetAction> GetNextBetAction(IList<Player> playerList)
        {
            var betActionTask = Observable.Merge(playerList.Select(p => p.BetCommand.Execute()))
                .FirstAsync(a => a != null)
                .ToTask();
            var betAction = await betActionTask;
            return betAction;
        }

        private static async Task<PlayAction> GetNextPlayAction(IList<Player> playerList)
        {
            var playActionTask = Observable.Merge(playerList.Select(p => p.PlayCommand.Execute()))
                .FirstAsync(a => a != null)
                .ToTask();
            var playAction = await playActionTask;
            return playAction;
        }
    }
}