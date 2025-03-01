using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Factory;
using PiggsCare.Core.Stores;
using PiggsCare.Core.ViewModels.Breeding;
using PiggsCare.Core.ViewModels.HealthRecords;

namespace PiggsCare.Core.ViewModels
{
    public class SelectedAnimalDetailsViewModel( IViewModelFactory viewModelFactory, ModalNavigationStore modalNavigationStore ):MvxViewModel<int>, ISelectedAnimalsDetailsViewModel
    {
        #region Properties

        public MvxViewModel? CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        #endregion

        public MvxCommand PowerOffCommand => new(ExecutePowerOff);

        private void ExecutePowerOff()
        {
            modalNavigationStore.Close();
        }

        public override void Prepare( int parameter )
        {
            _animalId = parameter;
            ExecuteNavigateToHealth();
        }

        #region Fields

        private MvxViewModel? _currentViewModel;
        private int _animalId;

        #endregion


        #region Commands

        public IMvxCommand NavigateToHealth => new MvxCommand(ExecuteNavigateToHealth);
        public IMvxCommand NavigateToBreeding => new MvxCommand(ExecuteNavigateToBreeding);

        #endregion

        #region Methods

        private void ExecuteNavigateToHealth()
        {
            HealthListingViewModel? viewmodel = viewModelFactory.CreateViewModel<HealthListingViewModel, int>(_animalId);
            CurrentViewModel = viewmodel;
            viewmodel?.Initialize();
        }

        private void ExecuteNavigateToBreeding()
        {
            BreedingEventListingViewModel? viewmodel = viewModelFactory.CreateViewModel<BreedingEventListingViewModel, int>(_animalId);
            CurrentViewModel = viewmodel;
            viewmodel?.Initialize();
        }

        #endregion
    }
}
