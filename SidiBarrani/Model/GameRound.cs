using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace SidiBarrani.Model
{
    public class GameRound
    {
        private Rules Rules {get;}
        private PlayerGroup PlayerGroup {get;}
        private Player InitialPlayer {get;}
        public GameRound(Rules rules, PlayerGroup playerGroup, Player initialPlayer) {
            Rules = rules;
            PlayerGroup = playerGroup;
            InitialPlayer = initialPlayer;
        }
        public RoundResult ProcessRound()
        {
            var cardPile = CardPile.CreateFullCardPile();
            var contextDictionary = DistributeCards(PlayerGroup, InitialPlayer, cardPile);
            foreach (var player in contextDictionary.Keys)
            {
                player.Context = contextDictionary[player];
            }
            var betResult = ProcessBetting(Rules, PlayerGroup, InitialPlayer);
            var playResult = ProcessPlaying(Rules, PlayerGroup, InitialPlayer, betResult.Bet.PlayType);
            var roundResult = DetermineRoundResult(Rules, PlayerGroup, betResult, playResult);
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

        private static BetResult ProcessBetting(Rules rules, PlayerGroup playerGroup, Player initialPlayer)
        {
            var betStage = new BetStage(rules, playerGroup, initialPlayer);
            var betResult = betStage.GetBetResult();
            while (betResult == null) {
                var playerList = playerGroup.GetPlayerListFromInitialPlayer(betStage.CurrentPlayer);
                UpdatePlayerContext(playerList, betStage);
                var betAction = GetNextBetAction(playerList).Result;
                betStage.AddBetActionAndProceed(betAction);
                betResult = betStage.GetBetResult();
            }
            return betResult;
        }

        private static PlayResult ProcessPlaying(Rules rules, PlayerGroup playerGroup, Player initialPlayer, PlayType playType)
        {
            var playStage = new PlayStage(rules, playerGroup, initialPlayer, playType);
            var playResult = playStage.GetPlayResult();
            while (playResult == null)
            {
                var playerList = playerGroup.GetPlayerListFromInitialPlayer(playStage.CurrentPlayer);
                UpdatePlayerContext(playerList, playStage);
                var playAction = GetNextPlayAction(playerList).Result;
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
            var betActionTasks = playerList
                .Select(p => Task.Run(() => p.BetActionGenerator(p.Context)))
                .ToList();
            BetAction betAction = null;
            while (betAction == null && betActionTasks.Any())
            {
                var task = await Task.WhenAny(betActionTasks);
                betAction = task.Result;
                betActionTasks.Remove(task);
            }
            return betAction;
            // var betActionTask = Observable.Merge(playerList.Select(p => p.BetCommand.Execute()))
            //     .FirstAsync(a => a != null)
            //     .ToTask();
            // return betActionTask.Result;
        }

        private static async Task<PlayAction> GetNextPlayAction(IList<Player> playerList)
        {
            var playActionTasks = playerList
                .Select(p => Task.Run(() => p.PlayActionGenerator(p.Context)))
                .ToList();
            PlayAction playAction = null;
            while (playAction == null && playActionTasks.Any())
            {
                var task = await Task.WhenAny(playActionTasks);
                playAction = task.Result;
                playActionTasks.Remove(task);
            }
            return playAction;
            // var playActionTask = Observable.Merge(playerList.Select(p => p.PlayCommand.Execute()))
            //     .FirstAsync(a => a != null)
            //     .ToTask();
            // return playActionTask.Result;
        }
    }
}