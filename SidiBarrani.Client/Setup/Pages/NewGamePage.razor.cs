using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SidiBarrani.Client.Setup.ViewModel;
using SidiBarrani.Shared.Model.Setup;
using SidiBarrani.Shared.Services;

namespace SidiBarrani.Client.Setup.Pages
{
    public partial class NewGamePage
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;
        [Inject]
        public IGameSetupService GameSetupService { get; set; } = null!;

        public NewGamePageViewModel NewGamePageViewModel { get; set; } = new ();

        protected async Task CreateGameCommand()
        {
            var gameSetup = await CreateNewGameSetup();
            NavigationManager.NavigateTo($"/gameSetup/{gameSetup.GameId}/");
        }

        private async Task<GameSetup> CreateNewGameSetup()
        {
            var gameSetup = await GameSetupService.OpenGameSetupAsync(NewGamePageViewModel.GameName!);
            StateHasChanged();

            return gameSetup;
        }
    }
}
