using PiggsCare.Core.Commands;
using System.Windows.Input;

namespace PiggsCare.Core.ViewModels
{
    public class MainViewModel:ViewModelBase, IMainViewModel
    {
        private ViewModelBase _currentViewModel = new AnalyticsViewModel();

        public MainViewModel()
        {
            NavigateToHome = new ChangeMainSectionCommand(new HomeViewModel(), this);
            NavigateToAnalytics = new ChangeMainSectionCommand(new AnalyticsViewModel(), this);
            NavigateToNotifications = new ChangeMainSectionCommand(new NotificationViewModel(), this);
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
