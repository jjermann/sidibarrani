using SidiBarrani.Shared.Model.Setup;

namespace SidiBarrani.Client.Setup.ViewModel
{
    public class GameSetupPageViewModel
    {
        public static GameSetupPageViewModel ConstructFromGameSetup(GameSetup gameSetup)
        {
            var gameSetupPageViewModel = new GameSetupPageViewModel
            {
                MinBet = gameSetup.Rules.MinBet,
                AllowUpDown = gameSetup.Rules.AllowUpDown,
                EndScore = gameSetup.Rules.EndScore,
                Team1Name = gameSetup.PlayerGroupSetup.Team1.TeamName,
                Team2Name = gameSetup.PlayerGroupSetup.Team2.TeamName
            };

            return gameSetupPageViewModel;
        }

        public int MinBet { get; set; }
        public bool AllowUpDown { get; set; }
        public int EndScore { get; set; }
        public string Team1Name { get; set; } = null!;
        public string Team2Name { get; set; } = null!;
    }
}
