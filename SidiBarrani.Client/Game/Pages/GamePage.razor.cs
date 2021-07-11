using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SidiBarrani.Client.Shared.Services;

namespace SidiBarrani.Client.Game.Pages
{
    public partial class GamePage : IDisposable
    {
        [Inject]
        public IClientConnectionService ClientConnectionService { get; set; } = null!;

        [Parameter]
        public string GameIdParam { get; set; } = null!;

        public Guid GameId => Guid.Parse(GameIdParam);

        protected override async Task OnInitializedAsync()
        {
            await ClientConnectionService.ConnectToHub();
            ClientConnectionService.GameContextChanged += OnGameContextChanged;
            ClientConnectionService.GameFinished += OnGameFinished;
        }

        private async Task OnGameContextChanged()
        {
            //TODO
            await Task.Run(() =>
            {
            });
            StateHasChanged();
        }

        private async Task OnGameFinished()
        {
            //TODO
            await Task.Run(() =>
            {
            });
            StateHasChanged();
        }

        public void Dispose()
        {
            ClientConnectionService.GameContextChanged -= OnGameContextChanged;
            ClientConnectionService.GameFinished -= OnGameFinished;
        }
    }
}
