using Avalonia;
using Avalonia.Markup.Xaml;

namespace SidiBarrani
{
    public class App :
        Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
   }
}