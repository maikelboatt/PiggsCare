using PiggsCare.UI.Themes;
using System.Windows;

namespace PiggsCare.UI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow:Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Themes_Click( object sender, RoutedEventArgs e )
        {
            ThemesController.SetTheme(Themes.IsChecked == true ? ThemeTypes.Dark : ThemeTypes.Light);
        }

        private void CloseButton_OnClick( object sender, RoutedEventArgs e )
        {
            Close();
        }

        private void RestoreButton_OnClick( object sender, RoutedEventArgs e )
        {
            WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void MinimizeButton_OnClick( object sender, RoutedEventArgs e )
        {
            WindowState = WindowState.Minimized;
        }
    }
}
