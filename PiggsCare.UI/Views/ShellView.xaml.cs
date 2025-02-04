using MvvmCross.Platforms.Wpf.Views;
using PiggsCare.UI.Themes;
using System.Windows;
using System.Windows.Input;

namespace PiggsCare.UI.Views
{
    public partial class ShellView:MvxWpfView
    {
        private Window _parentWindow;

        public ShellView()
        {
            _parentWindow = Window.GetWindow(this);
            InitializeComponent();
        }

        private void Themes_Click( object sender, RoutedEventArgs e )
        {
            ThemesController.SetTheme(Themes.IsChecked == true ? ThemeTypes.Dark : ThemeTypes.Light);
        }

        private void CloseButton_OnClick( object sender, RoutedEventArgs e )
        {
            _parentWindow?.Close();
        }

        private void RestoreButton_OnClick( object sender, RoutedEventArgs e )
        {
            _parentWindow.WindowState = _parentWindow.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void MinimizeButton_OnClick( object sender, RoutedEventArgs e )
        {
            _parentWindow.WindowState = WindowState.Minimized;
        }

        private void ShellView_OnLoaded( object sender, RoutedEventArgs e )
        {
            _parentWindow = Window.GetWindow(this);
        }

        private void Border_OnMouseDown( object sender, MouseButtonEventArgs e )
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                _parentWindow.DragMove();
        }

        private void UIElement_OnMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            _parentWindow.DragMove();
        }
    }
}
