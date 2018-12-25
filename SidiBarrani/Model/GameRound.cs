using System;
using System.Linq;

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
            DistributeCards(PlayerGroup, InitialPlayer);
            var betResult = ProcessBetting(Rules, PlayerGroup, InitialPlayer);
            var playResult = ProcessPlaying(Rules, PlayerGroup, InitialPlayer, betResult.Bet.PlayType);
            var roundResult = DetermineRoundResult(Rules, PlayerGroup, betResult, playResult);
            return roundResult;
        }

        private static void DistributeCards(PlayerGroup playerGroup, Player initialPlayer)
        {
            var cardPile = CardPile.CreateFullCardPile();
            var playerOrder = playerGroup.GetPlayerListFromInitialPlayer(initialPlayer);
            foreach (var player in playerOrder)
            {
                var playerContext = new PlayerContext
                {
                    CardsInHand = cardPile.Draw(9)
                };
                player.Context = playerContext;
            }
        }

        private static BetResult ProcessBetting(Rules rules, PlayerGroup playerGroup, Player initialPlayer)
        {
            var betStage = new BetStage(rules, playerGroup, initialPlayer);
            var betResult = betStage.GetBetResult();
            while (betResult == null) {
                var validActionDictionary = betStage
                    .GetValidBetActions()
                    .GroupBy(a => a.Player)
                    .ToDictionary(g => g.Key, g => g.ToList());
                // foreach (var player in validActionDictionary.Keys) {
                //     var validPlayerActions = validActionDictionary[player];
                //     //Create a task for player to return an action with cancelation token in case an action occurs somewhere else (or something of that sort)
                //     //-> Figure out how to handle time for Sidi/Barrani!
                // }
                var randomAction = validActionDictionary.Values
                    .SelectMany(l => l)
                    .OrderBy(a => Guid.NewGuid())
                    .FirstOrDefault();
                betStage.AddBetActionAndProceed(randomAction);
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
                var validActionDictionary = playStage
                    .GetValidPlayActions()
                    .GroupBy(a => a.Player)
                    .ToDictionary(g => g.Key, g => g.ToList());
                // foreach (var player in validActionDictionary.Keys) {
                //     var validPlayerActions = validActionDictionary[player];
                // }
                var randomAction = validActionDictionary.Values
                    .SelectMany(l => l)
                    .OrderBy(a => Guid.NewGuid())
                    .FirstOrDefault();
                playStage.AddPlayActionAndProceed(randomAction);
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