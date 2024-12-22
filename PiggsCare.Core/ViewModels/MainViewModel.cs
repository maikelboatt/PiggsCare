using PiggsCare.Core.Commands.NavigationCommands;
using System.Windows.Input;

namespace PiggsCare.Core.ViewModels
{
    public class MainViewModel:ViewModelBase, IMainViewModel
    {
        private ViewModelBase _currentViewModel = new AnalyticsViewModel();

        public MainViewModel()
        {
            NavigateToHome = new NavigateToHomeCommand(this);
            NavigateToAnalytics = new NavigateToAnalyticsCommand(this);
            NavigateToNotifications = new NavigateToNotificationCommand(this);
        }

        #region Properties

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetField(ref _currentViewModel, value);
        }

        #endregion

        #region Commands

        public ICommand NavigateToHome { get; private set; }
        public ICommand NavigateToAnalytics { get; private set; }
        public ICommand NavigateToNotifications { get; private set; }

        #endregion
    }
}
