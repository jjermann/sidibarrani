using System;
using System.Collections.Generic;
using SidiBarrani.Shared.Model.Game;

namespace SidiBarrani.Shared.Model.Setup
{
    public class GameSetup
    {
        public static GameSetup Construct(string gameName)
        {
            return new GameSetup
            {
                VersionId = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                GameName = gameName,
                Rules = new Rules(),
                PlayerGroupSetup = PlayerGroupSetup.ConstructDefault(),
                CreationDate = DateTime.Today
            };
        }

        public Guid VersionId { get; set; }
        public Guid GameId { get; init; }
        public string GameName { get; init; } = null!;
        public Rules Rules { get; set; } = null!;
        public PlayerGroupSetup PlayerGroupSetup { get; init; } = null!;
        public DateTime CreationDate { get; private init; }

        public IList<PlayerSetup> GetPlayerList()
        {
            var team1Player1 = PlayerGroupSetup.Team1.Player1;
            var team1Player2 = PlayerGroupSetup.Team1.Player2;
            var team2Player1 = PlayerGroupSetup.Team2.Player1;
            var team2Player2 = PlayerGroupSetup.Team2.Player2;
            var playerList = new List<PlayerSetup>();
            if (team1Player1 != null)
            {
                playerList.Add(team1Player1);
            }
            if (team1Player2 != null)
            {
                playerList.Add(team1Player2);
            }
            if (team2Player1 != null)
            {
                playerList.Add(team2Player1);
            }
            if (team2Player2 != null)
            {
                playerList.Add(team2Player2);
            }

            return playerList;
        }

        public bool HasOpenSpots()
        {
            var openSpots = GetOpenSpots();

            return openSpots > 0;
        }

        public int GetOpenSpots()
        {
            var playerList = GetPlayerList();

            return 4 - playerList.Count;
        }

        public void AddPlayer(PlayerSetup playerSetup)
        {
            if (!HasOpenSpots())
            {
                throw new InvalidOperationException();
            }

            if (PlayerGroupSetup.Team1.Player1 == null)
            {
                PlayerGroupSetup.Team1.Player1 = playerSetup;
            }
            else if (PlayerGroupSetup.Team1.Player2 == null)
            {
                PlayerGroupSetup.Team1.Player2 = playerSetup;
            }
            else if (PlayerGroupSetup.Team2.Player1 == null)
            {
                PlayerGroupSetup.Team2.Player1 = playerSetup;
            }
            else if (PlayerGroupSetup.Team2.Player2 == null)
            {
                PlayerGroupSetup.Team2.Player2 = playerSetup;
            }
            VersionId = Guid.NewGuid();
        }

        public Rules SetMinBetRule(int minBet)
        {
            var newRules = Rules with {MinBet = minBet};
            Rules = newRules;
            VersionId = Guid.NewGuid();

            return newRules;
        }

        public Rules SetAllowUpDownRule(bool allowUpDown)
        {
            var newRules = Rules with { AllowUpDown = allowUpDown };
            Rules = newRules;
            VersionId = Guid.NewGuid();

            return newRules;
        }

        public Rules SetEndScoreRule(int endScore)
        {
            var newRules = Rules with { EndScore = endScore };
            Rules = newRules;
            VersionId = Guid.NewGuid();

            return newRules;
        }

        public bool IsReady()
        {
            return !HasOpenSpots();
        }

        public override string ToString() => GameName;
    }
}
