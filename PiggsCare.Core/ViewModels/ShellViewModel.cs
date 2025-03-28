using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Factory;
using PiggsCare.Core.Stores;

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
            NavigateToNotifications = new MvxCommand(ShowNotificationView);
            NavigateToSynchronization = new MvxCommand(ShowSynchronizationView);

            modalNavigationStore.CurrentModalViewModelChanged += ModalNavigationStoreOnCurrentModalViewModelChanged;

        }

        public override Task Initialize()
        {
            ShowAnalyticsView();
            return base.Initialize();
        }

        private void ShowNotificationView()
        {
            NotificationViewModel? viewmodel = _viewModelFactory.CreateViewModel<NotificationViewModel>();
            CurrentViewModel = viewmodel;
            viewmodel?.Prepare();
            viewmodel?.Initialize();
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
        public MvxCommand NavigateToNotifications { get; private set; }

        public MvxCommand NavigateToSynchronization { get; private set; }

        #endregion
    }
}
