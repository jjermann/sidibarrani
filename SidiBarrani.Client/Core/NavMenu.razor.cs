using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SidiBarrani.Client.Shared.Services;

namespace SidiBarrani.Client.Core
{
    public partial class NavMenu: IDisposable
    {
        [Inject]
        public IClientConnectionService ClientConnectionService { get; set; } = null!;

        private bool _collapseNavMenu = true;
        private string? CurrentGameStageLink => ClientConnectionService.CurrentGameSetup != null
            ? $"/gameSetup/{ClientConnectionService.CurrentGameSetup.GameId}"
            : null;

        private string? NavMenuCssClass => _collapseNavMenu ? "collapse" : null;

        protected override void OnInitialized()
        {
            ClientConnectionService.OnChange += StateHasChanged;
        }

        private void ToggleNavMenu()
        {
            _collapseNavMenu = !_collapseNavMenu;
        }

        public void Dispose()
        {
            ClientConnectionService.OnChange -= StateHasChanged;
        }
    }
}
