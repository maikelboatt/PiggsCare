using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Factory;
using PiggsCare.Core.Stores;
using PiggsCare.Core.ViewModels.Breeding;
using PiggsCare.Core.ViewModels.HealthRecords;
using PiggsCare.Core.ViewModels.Removal;
using PiggsCare.Core.ViewModels.Synchronization;
using PiggsCare.Domain.Services;
using System.Windows;

namespace PiggsCare.Core.ViewModels
{
    public class SelectedAnimalDetailsViewModel:MvxViewModel<int>, ISelectedAnimalsDetailsViewModel
    {
        #region Constructor

        public SelectedAnimalDetailsViewModel( IViewModelFactory viewModelFactory, ModalNavigationStore modalNavigationStore, ICurrentViewModelStore currentViewModelStore,
            IMessageService messageService )
        {
            _viewModelFactory = viewModelFactory;
            _modalNavigationStore = modalNavigationStore;
            _currentViewModelStore = currentViewModelStore;
            _messageService = messageService;

            _currentViewModelStore.CurrentViewModelChanged += CurrentViewModelStoreOnCurrentViewModelChanged;
            _currentViewModelStore.CurrentProcessStageChanged += CurrentViewModelStoreOnCurrentProcessStageChanged;
        }

        #endregion

        public MvxCommand PowerOffCommand => new(ExecutePowerOff);

        private void ExecutePowerOff()
        {
            _modalNavigationStore.Close();
        }

        #region Event Handler

        private void CurrentViewModelStoreOnCurrentViewModelChanged()
        {
            RaisePropertyChanged(nameof(CurrentViewModel));
        }

        private void CurrentViewModelStoreOnCurrentProcessStageChanged()
        {
            RaisePropertyChanged(nameof(CurrentProcessStage));
        }

        #endregion

        #region Properties

        public MvxViewModel? CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (value == _currentViewModel) return;
                _currentViewModelStore.CurrentViewModel = value;
            }
        }

        public ProcessStage CurrentProcessStage
        {
            get => _currentStage;
            set
            {
                if (value == _currentStage) return;
                _currentViewModelStore.CurrentProcessStage = value;
            }
        }

        #endregion

        #region ViewModel Life-Cycle

        public override void Prepare( int parameter )
        {
            _animalId = parameter;
            ExecuteNavigateToHealth();
        }

        public override void ViewDestroy( bool viewFinishing = true )
        {
            base.ViewDestroy(viewFinishing);
            _currentViewModelStore.CurrentViewModelChanged -= CurrentViewModelStoreOnCurrentViewModelChanged;
            _currentViewModelStore.CurrentProcessStageChanged -= CurrentViewModelStoreOnCurrentProcessStageChanged;
        }

        #endregion

        #region Fields

        private MvxViewModel? _currentViewModel => _currentViewModelStore?.CurrentViewModel;
        private ProcessStage _currentStage => _currentViewModelStore.CurrentProcessStage;
        private int _animalId;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly ICurrentViewModelStore _currentViewModelStore;
        private readonly IMessageService _messageService;

        #endregion

        #region Commands

        public IMvxCommand NavigateToHealth => new MvxCommand(ExecuteNavigateToHealth);
        public IMvxCommand NavigateToSynchronization => new MvxCommand(ExecuteNavigateToSynchronization);
        public IMvxCommand NavigateToBreeding => new MvxCommand(ExecuteNavigateToBreeding);
        public IMvxCommand NavigateToFarrowing => new MvxCommand(ExecuteNavigateToFarrowing);
        public IMvxCommand NavigateToPregnancy => new MvxCommand(ExecuteNavigateToPregnancy);
        public IMvxCommand NavigateToWeaning => new MvxCommand(ExecuteNavigateToWeaning);
        public IMvxCommand NavigateToRemoval => new MvxCommand(ExecuteNavigateToRemoval);

        #endregion

        #region Methods

        private void ExecuteNavigateToHealth()
        {
            HealthListingViewModel? viewmodel = _viewModelFactory.CreateViewModel<HealthListingViewModel, int>(_animalId);
            _currentViewModelStore.CurrentViewModel = viewmodel;
            CurrentProcessStage = ProcessStage.Health;
            viewmodel?.Initialize();
        }

        private void ExecuteNavigateToSynchronization()
        {
            SynchronizationListingViewModel? viewmodel = _viewModelFactory.CreateViewModel<SynchronizationListingViewModel, int>(_animalId);
            _currentViewModelStore.CurrentViewModel = viewmodel;
            CurrentProcessStage = ProcessStage.Synchronization;
            viewmodel?.Initialize();
        }

        private void ExecuteNavigateToBreeding()
        {
            BreedingEventListingViewModel? viewmodel = _viewModelFactory.CreateViewModel<BreedingEventListingViewModel, int>(_animalId);
            _currentViewModelStore.CurrentViewModel = viewmodel;
            CurrentProcessStage = ProcessStage.Breeding;
            viewmodel?.Initialize();
        }

        private void ExecuteNavigateToFarrowing()
        {
            if (_messageService.Show(
                    "Please select the second of two option under Navigation to successfully view data on farrowing",
                    "Information",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information) != MessageBoxResult.OK)
            {
                return;
            }

            CurrentProcessStage = ProcessStage.Farrowing;
            ExecuteNavigateToBreeding();
        }

        private void ExecuteNavigateToPregnancy()
        {
            if (_messageService.Show(
                    "Please select the first of two options under under Navigation to successfully view data on pregnancy",
                    "Information",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information) != MessageBoxResult.OK)
            {
                return;
            }

            CurrentProcessStage = ProcessStage.Pregnancy;
            ExecuteNavigateToBreeding();
        }

        private void ExecuteNavigateToWeaning()
        {
            if (_messageService.Show(
                    "Please select the option under Weaning to successfully view data on weaning",
                    "Information",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information) != MessageBoxResult.OK)
            {
                return;
            }

            CurrentProcessStage = ProcessStage.Weaning;
            ExecuteNavigateToFarrowing();
        }

        private void ExecuteNavigateToRemoval()
        {
            RemovalListingViewModel? viewmodel = _viewModelFactory.CreateViewModel<RemovalListingViewModel, int>(_animalId);
            _currentViewModelStore.CurrentViewModel = viewmodel;
            CurrentProcessStage = ProcessStage.Removal;
            viewmodel?.Initialize();
        }

        #endregion
    }
}
