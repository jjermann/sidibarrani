
using Microsoft.AspNetCore.Components;
using SidiBarrani.Shared.Services;

namespace SidiBarrani.Client.Shared
{
    public partial class NavMenu
    {
        [Inject]
        public IClientConnectionService ClientConnectionService { get; set; } = null!;

        private bool _collapseNavMenu = true;

        private string? NavMenuCssClass => _collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            _collapseNavMenu = !_collapseNavMenu;
        }
    }
}
