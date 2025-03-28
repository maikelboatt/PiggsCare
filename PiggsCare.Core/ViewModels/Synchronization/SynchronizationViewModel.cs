using MvvmCross.ViewModels;
using PiggsCare.Core.Factory;

namespace PiggsCare.Core.ViewModels.Synchronization
{
    public class SynchronizationViewModel( IViewModelFactory viewModelFactory ):MvxViewModel, ISynchronizationViewModel
    {
        public SynchronizationListingViewModel? SynchronizationListingViewModel => CreateViewModel();

        private SynchronizationListingViewModel? CreateViewModel()
        {
            // Create a new instance of SynchronizationListingViewModel
            SynchronizationListingViewModel? viewmodel = viewModelFactory.CreateViewModel<SynchronizationListingViewModel>();
            viewmodel?.Initialize();
            return viewmodel;
        }
    }
}
