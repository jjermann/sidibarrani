using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SidiBarrani.Shared.Model.Setup;
using SidiBarrani.Shared.Services;

namespace SidiBarrani.Client.Setup.Pages
{
    public partial class GameSetupListPage: IDisposable
    {
        [Inject]
        public IGameSetupService GameSetupService { get; set; } = null!;
        [Inject]
        public IClientConnectionService ClientConnectionService { get; set; } = null!;

        public IList<GameSetup>? GameSetupList { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ClientConnectionService.ConnectToHub();
            var gameSetupList = await GameSetupService.GetGameSetupListAsync();
            GameSetupList = gameSetupList;

            ClientConnectionService.GameSetupListChanged += OnGameSetupListChanged;
        }

        private async Task OnGameSetupListChanged()
        {
            var gameSetupList = await GameSetupService.GetGameSetupListAsync();
            GameSetupList = gameSetupList;
            StateHasChanged();
        }

        public void Dispose()
        {
            ClientConnectionService.GameSetupListChanged -= OnGameSetupListChanged;
        }
    }
}
