using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Factory;
using PiggsCare.Core.Stores;
using PiggsCare.Core.ViewModels.HealthRecords;

namespace PiggsCare.Core.ViewModels
{
    public class SelectedAnimalDetailsViewModel( IViewModelFactory viewModelFactory, ModalNavigationStore modalNavigationStore ):MvxViewModel<int>, ISelectedAnimalsDetailsViewModel
    {
        #region Fields

        private MvxViewModel? _currentViewModel;

        #endregion

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
            SetupViewModel(parameter);
        }


        private void SetupViewModel( int parameter )
        {
            HealthListingViewModel? viewmodel = viewModelFactory.CreateViewModel<HealthListingViewModel, int>(parameter);
            CurrentViewModel = viewmodel;
            viewmodel?.Initialize();
        }
    }
}
