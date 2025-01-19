using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PiggsCare.Core.Commands;
using PiggsCare.Domain.Services;
using System.Windows.Input;

namespace PiggsCare.Core.ViewModels
{
    public class ShellViewModel:MvxViewModel, IMainViewModel
    {
        private MvxViewModel _currentViewModel = new AnalyticsViewModel();

        public ShellViewModel( IAnimalService animalService, IMvxNavigationService navigationService, IMvxMessenger messenger )
        {
            NavigateToHome = new ChangeMainSectionCommand(new HomeViewModel(animalService, navigationService, messenger), this);
            NavigateToAnalytics = new ChangeMainSectionCommand(new AnalyticsViewModel(), this);
            NavigateToNotifications = new ChangeMainSectionCommand(new NotificationViewModel(), this);
        }


        #region Properties

        public MvxViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        #endregion

        #region Commands

        public ICommand NavigateToHome { get; private set; }
        public ICommand NavigateToAnalytics { get; private set; }
        public ICommand NavigateToNotifications { get; private set; }

        #endregion
    }
}
