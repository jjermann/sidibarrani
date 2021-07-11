using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SidiBarrani.Client.Setup.ViewModel;
using SidiBarrani.Client.Shared.Services;
using SidiBarrani.Shared.Model.Setup;
using SidiBarrani.Shared.Services;

namespace SidiBarrani.Client.Setup.Pages
{
    public partial class GameSetupPage : IDisposable
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;
        [Inject]
        public IGameSetupService GameSetupService { get; set; } = null!;
        [Inject]
        public IClientConnectionService ClientConnectionService { get; set; } = null!;

        [Parameter]
        public string GameIdParam { get; set; } = null!;

        public Guid GameId => Guid.Parse(GameIdParam);
        public GameSetup? GameSetup { get; set; }
        public EditContext? CurrentEditContext { get; set; }
        public GameSetupPageViewModel? GameSetupPageViewModel { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await ClientConnectionService.ConnectToHub();
            await UpdateGameSetupFromApi();

            ClientConnectionService.GameStageChanged += OnGameStageChanged;
            ClientConnectionService.GameStarted += OnGameStarted;
        }

        private async Task OnGameStageChanged()
        {
            await UpdateGameSetupFromApi();
        }

        private async Task OnGameStarted()
        {
            await Task.Run(() =>
            {
            });
            NavigationManager.NavigateTo($"/game/{GameId}/");
        }

        private async Task JoinGameSetupCommand()
        {
            if (GameSetup == null)
            {
                throw new InvalidOperationException();
            }

            var playerConnectionId = await ClientConnectionService.GetPlayerConnectionId();
            await ClientConnectionService.ConnectToGameSetup(GameSetup);
            var playerSetup = new PlayerSetup(playerConnectionId, $"PlayerName {DateTime.Now.Ticks}");
            await GameSetupService.JoinGameSetupAsync(GameId, playerSetup, GameSetup.VersionId);
            StateHasChanged();
        }

        private async Task UpdateGameSetupCommand()
        {
            await UpdateGameSetup();
        }

        private async Task StartGameCommand()
        {
            if (GameSetup == null)
            {
                throw new InvalidOperationException();
            }

            await GameSetupService.StartGameAsync(GameId, GameSetup.VersionId);
            NavigationManager.NavigateTo($"/game/{GameId}/");
        }

        private async Task UpdateGameSetupFromApi()
        {
            GameSetup = await GameSetupService.GetGameSetupAsync(GameId);
            GameSetupPageViewModel = GameSetup != null
                ? GameSetupPageViewModel.ConstructFromGameSetup(GameSetup)
                : null;
            StateHasChanged();
        }

        private async Task UpdateGameSetup()
        {
            //TODO
            await UpdateGameSetupFromApi();
        }

        public void Dispose()
        {
            ClientConnectionService.GameStageChanged -= OnGameStageChanged;
            ClientConnectionService.GameStarted -= OnGameStarted;
        }
    }
}
