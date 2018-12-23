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
            InitializeComponent();
            this.AttachDevTools();
        }


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}