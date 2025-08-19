using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores;
using PiggsCare.Core.Factory;

namespace PiggsCare.Core.ViewModels
{
    public class ShellViewModel:MvxViewModel, IMainViewModel
    {
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly IViewModelFactory _viewModelFactory;
        private MvxViewModel? _currentViewModel;

        public ShellViewModel( ModalNavigationStore modalNavigationStore, IViewModelFactory viewModelFactory )
        {
            _modalNavigationStore = modalNavigationStore;
            _viewModelFactory = viewModelFactory;
            NavigateToHome = new MvxCommand(ShowHomeView);
            NavigateToAnalytics = new MvxCommand(ShowAnalyticsView);
            NavigateToSynchronization = new MvxCommand(ShowSynchronizationView);
            NavigateToReminders = new MvxCommand(ShowReminderView);
            NavigateToNotifications = new MvxCommand(ShowNotificationView);

            modalNavigationStore.CurrentModalViewModelChanged += ModalNavigationStoreOnCurrentModalViewModelChanged;

        }

        public override Task Initialize()
        {
            ShowAnalyticsView();
            return base.Initialize();
        }


        private void ShowHomeView()
        {
            HomeViewModel? viewmodel = _viewModelFactory.CreateViewModel<HomeViewModel>();
            CurrentViewModel = viewmodel;
            viewmodel?.Initialize();
        }

        private void ShowAnalyticsView()
        {
            AnalyticsViewModel? viewmodel = _viewModelFactory.CreateViewModel<AnalyticsViewModel>();
            CurrentViewModel = viewmodel;
            viewmodel?.Initialize();
        }

        private void ShowSynchronizationView()
        {
            SynchronizationViewModel? viewmodel = _viewModelFactory.CreateViewModel<SynchronizationViewModel>();
            CurrentViewModel = viewmodel;
            viewmodel?.Initialize();
        }

        private void ShowReminderView()
        {
            ReminderViewModel? viewModel = _viewModelFactory.CreateViewModel<ReminderViewModel>();
            CurrentViewModel = viewModel;
            viewModel?.Initialize();
        }

        private void ShowNotificationView()
        {
            NotificationViewModel? viewmodel = _viewModelFactory.CreateViewModel<NotificationViewModel>();
            CurrentViewModel = viewmodel;
            viewmodel?.Prepare();
            viewmodel?.Initialize();
        }


        private void ModalNavigationStoreOnCurrentModalViewModelChanged()
        {
            RaisePropertyChanged(nameof(CurrentModalViewModel));
            RaisePropertyChanged(nameof(IsModalOpen));
        }

        #region Properties

        public MvxViewModel? CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public bool IsModalOpen => _modalNavigationStore.CurrentModalViewModel != null;
        public MvxViewModel? CurrentModalViewModel => _modalNavigationStore?.CurrentModalViewModel;

        #endregion

        #region Commands

        public MvxCommand NavigateToHome { get; private set; }
        public MvxCommand NavigateToAnalytics { get; private set; }
        public MvxCommand NavigateToSynchronization { get; private set; }
        public MvxCommand NavigateToReminders { get; private set; }
        public MvxCommand NavigateToNotifications { get; private set; }

        #endregion
    }
}
