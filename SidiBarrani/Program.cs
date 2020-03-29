using Avalonia;
using Avalonia.ReactiveUI;
using Serilog;
using SidiBarrani.View;
using SidiBarrani.ViewModel;
using SidiBarraniServer;

namespace SidiBarrani
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();
            var sidiBarraniServer = new SidiBarraniServerImplementation();
            BuildAvaloniaApp()
                .Start<GameView>(() => new SidiBarraniViewModel(sidiBarraniServer));
        }

        public static AppBuilder BuildAvaloniaApp() => AppBuilder
            .Configure<App>()
            .UseReactiveUI()
            .UsePlatformDetect();
    }
}
