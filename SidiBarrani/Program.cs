using System;
using Avalonia;
using SidiBarrani.View;
using SidiBarrani.ViewModel;

namespace SidiBarrani
{
    public class Program
    {
        static void Main(string[] args)
        {
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .Start<BoardView>(() => new SidiBarraniViewModel());
        }
    }
}
