using System.IO;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SidiBarrani.ViewModel;

namespace SidiBarrani.View
{
    public class BoardView :
        Window
    {
        public BoardView()
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)); 
            InitializeComponent();
            this.AttachDevTools();
        }


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}