using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SidiBarrani.Client.Shared.Services;

namespace SidiBarrani.Client.Core.Components
{
    public partial class LoginDialog
    {
        [Inject]
        private IClientConnectionService ClientConnectionService { get; set; } = null!;

        private bool ShowDialog => !ClientConnectionService.IsLoggedIn;
        private string UserName { get; set; } = "";

        private async Task LoginCommand()
        {
            await ClientConnectionService.Login(UserName);
            StateHasChanged();
        }
    }
}