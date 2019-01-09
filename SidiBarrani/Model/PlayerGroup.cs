using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;

namespace SidiBarrani.Model
{
    public class PlayerGroup
    {
        public Team Team1 {get;}
        public Team Team2 {get;}

        private PlayerGroup() { }
        public PlayerGroup(Team team1, Team team2)
        {
            Team1 = team1;
            Team2 = team2;
        }

        public ReactiveCommand<BetStage, BetAction> RequestBetCommand {get;set;}
        public ReactiveCommand<PlayStage, PlayAction> RequestPlayCommand {get;set;}
        public ReactiveCommand<Unit, Unit> RequestConfirmCommand {get;set;}

        public void AttachCommands()
        {
            var playerList = GetPlayerList();
            var betActionObservable = Observable.Merge(playerList.Select(p => p.RequestBetCommand.Execute()))
                .FirstAsync(a => a != null);
            var playActionObservable = Observable.Merge(playerList.Select(p => p.RequestPlayCommand.Execute()))
                .FirstAsync(a => a != null);
            var lastConfirmationObservable = Observable.Merge(playerList.Select(p =>p.RequestConfirmCommand.Execute()))
                .LastAsync();
            RequestBetCommand = ReactiveCommand.CreateFromTask<BetStage, BetAction>(betStage => Task.Run(async () =>
            {
                foreach (var player in playerList) {
                    player.Context.AvailableBetActions.Clear();
                }
                var validActionDictionary = betStage
                    .GetValidBetActions()
                    .GroupBy(a => a.Player)
                    .ToDictionary(g => g.Key, g => g.ToList());
                foreach (var player in playerList) {
                    var hasActions = validActionDictionary.ContainsKey(player);
                    if (hasActions)
                    {
                        player.Context.AvailableBetActions.AddRange(validActionDictionary[player]);
                    }
                    player.Context.IsCurrentPlayer = player == betStage.CurrentPlayer;
                }
                var betAction = await betActionObservable;
                foreach (var player in playerList) {
                    player.Context.AvailableBetActions.Clear();
                }
                return betAction;
            }));
            RequestPlayCommand = ReactiveCommand.CreateFromTask<PlayStage, PlayAction>(playStage => Task.Run(async () =>
            {
                foreach (var player in playerList) {
                    player.Context.AvailablePlayActions.Clear();
                }
                var validActionDictionary = playStage
                    .GetValidPlayActions()
                    .GroupBy(a => a.Player)
                    .ToDictionary(g => g.Key, g => g.ToList());
                foreach (var player in playerList) {
                    var hasActions = validActionDictionary.ContainsKey(player);
                    if (hasActions)
                    {
                        player.Context.AvailablePlayActions.AddRange(validActionDictionary[player]);
                    }
                    player.Context.IsCurrentPlayer = player == playStage.CurrentPlayer;
                }
                var playAction = await playActionObservable;
                foreach (var player in playerList) {
                    player.Context.AvailablePlayActions.Clear();
                }
                return playAction;
            }));
            RequestConfirmCommand = ReactiveCommand.CreateFromTask(() => Task.Run(async () =>
            {
                foreach (var player in playerList) {
                    player.Context.CanConfirm = true;
                }
                await lastConfirmationObservable;
                foreach (var player in playerList) {
                    player.Context.CanConfirm = false;
                }
            }));
        }

        public IList<Player> GetPlayerList()
        {
            var playerList = new List<Player>
            {
                Team1.Player1,
                Team2.Player1,
                Team1.Player2,
                Team2.Player2
            };
            return playerList;
        }

        public IList<Player> GetPlayerListFromInitialPlayer(Player initialPlayer)
        {
            var playerList = GetPlayerList();
            while (playerList.First() != initialPlayer)
            {
                var tmpFirstPlayer = playerList.First();
                playerList.RemoveAt(0);
                playerList.Add(tmpFirstPlayer);
            }
            return playerList;
        }

        public Player GetRandomPlayer()
        {
            var randomPlayer = GetPlayerList()
                .OrderBy(p => Guid.NewGuid())
                .First();
            return randomPlayer;
        }

        public Player GetNextPlayer(Player previousPlayer)
        {
            var playerList = GetPlayerList();
            var previousIndex = playerList.IndexOf(previousPlayer);
            var nextIndex = previousIndex == 3
                ? 0
                : previousIndex + 1;
            return playerList[nextIndex];
        }

        public Team GetOtherTeam(Team team)
        {
            return team == Team1
                ? Team2
                : Team1;
        }

        public Team GetOtherTeam(Player player)
        {
            return GetOtherTeam(player.Team);
        }
    }
}