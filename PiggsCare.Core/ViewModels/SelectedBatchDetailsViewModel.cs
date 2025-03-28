using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Factory;
using PiggsCare.Core.Stores;
using PiggsCare.Core.ViewModels.Breeding;
using PiggsCare.Domain.Services;
using System.Windows;

namespace PiggsCare.Core.ViewModels
{
    public class SelectedBatchDetailsViewModel:MvxViewModel<int>, ISelectedBatchDetailsViewModel
    {
        #region Constructor

        public SelectedBatchDetailsViewModel( IViewModelFactory viewModelFactory, ModalNavigationStore modalNavigationStore, ICurrentViewModelStore currentViewModelStore,
            IMessageService messageService )
        {
            _viewModelFactory = viewModelFactory;
            _modalNavigationStore = modalNavigationStore;
            _currentViewModelStore = currentViewModelStore;
            _messageService = messageService;

            // Subscribe to the CurrentViewModelChanged event of the _currentViewModelStore
            _currentViewModelStore.CurrentViewModelChanged += CurrentViewModelStoreOnCurrentViewModelChanged;

            // Subscribe to the CurrentProcessStageChanged event of the _currentViewModelStore
            _currentViewModelStore.CurrentProcessStageChanged += CurrentViewModelStoreOnCurrentProcessStageChanged;
        }

        #endregion

        #region Methods

        private void ExecuteNavigateToInsemination()
        {
            BreedingBatchListingViewModel? viewmodel = _viewModelFactory.CreateViewModel<BreedingBatchListingViewModel, int>(_synchronizationId);
            _currentViewModelStore.CurrentViewModel = viewmodel;
            _currentViewModelStore.CurrentProcessStage = ProcessStage.Breeding;
            viewmodel?.Initialize();
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
            ExecuteNavigateToInsemination();
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
            ExecuteNavigateToInsemination();
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

        private void ExecutePowerOffCommand()
        {
            _modalNavigationStore.Close();
        }

        #endregion

        #region Commands

        public IMvxCommand NavigateToInsemination => new MvxCommand(ExecuteNavigateToInsemination);
        public IMvxCommand NavigateToPregnancy => new MvxCommand(ExecuteNavigateToPregnancy);
        public IMvxCommand NavigateToFarrowing => new MvxCommand(ExecuteNavigateToFarrowing);
        public IMvxCommand NavigateToWeaning => new MvxCommand(ExecuteNavigateToWeaning);

        public MvxCommand PowerOffCommand => new(ExecutePowerOffCommand);

        #endregion

        #region ViewModel LifeCycle

        public override void Prepare( int parameter )
        {
            _synchronizationId = parameter;
            ExecuteNavigateToInsemination();
        }

        public override void ViewDestroy( bool viewFinishing = true )
        {
            base.ViewDestroy(viewFinishing);
            _currentViewModelStore.CurrentViewModelChanged -= CurrentViewModelStoreOnCurrentViewModelChanged;
            _currentViewModelStore.CurrentProcessStageChanged -= CurrentViewModelStoreOnCurrentProcessStageChanged;
        }

        #endregion

        #region Fields

        private MvxViewModel? _currentViewModel => _currentViewModelStore.CurrentViewModel;
        private ProcessStage _currentProcessStage => _currentViewModelStore.CurrentProcessStage;

        private int _synchronizationId;

        private readonly ICurrentViewModelStore _currentViewModelStore;
        private readonly IMessageService _messageService;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly IViewModelFactory _viewModelFactory;

        #endregion

        #region Event Handlers

        private void CurrentViewModelStoreOnCurrentProcessStageChanged()
        {
            RaisePropertyChanged(nameof(CurrentProcessStage));
        }

        private void CurrentViewModelStoreOnCurrentViewModelChanged()
        {
            RaisePropertyChanged(nameof(CurrentViewModel));
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
            get => _currentProcessStage;
            set
            {
                if (value == _currentProcessStage) return;
                _currentViewModelStore.CurrentProcessStage = value;
            }
        }

        #endregion
    }
}
