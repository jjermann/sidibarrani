using System;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using SidiBarrani.Client.Core;
using SidiBarrani.Client.Setup.Services;
using SidiBarrani.Client.Shared.Services;
using SidiBarrani.Shared.Services;

namespace SidiBarrani.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            var baseAddress = builder.HostEnvironment.BaseAddress.TrimEnd('/');
            builder.Services.AddHttpClient<IGameSetupService, ClientGameSetupService>(
                client => client.BaseAddress = new Uri(baseAddress));
            builder.Services.AddSingleton<IClientConnectionService, ClientConnectionService>(_ => new ClientConnectionService(baseAddress));

            await builder.Build().RunAsync();
        }
    }
}
