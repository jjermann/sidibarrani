using Microsoft.AspNetCore.Components;

namespace SidiBarrani.Client
{
    public partial class Index
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        protected override void OnInitialized()
        {
            NavigationManager.NavigateTo("/gameSetup");
        }
    }
}
