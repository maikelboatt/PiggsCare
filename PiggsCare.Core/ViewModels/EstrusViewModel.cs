using MvvmCross.ViewModels;
using PiggsCare.Core.Factory;
using PiggsCare.Core.ViewModels.Synchronization;

namespace PiggsCare.Core.ViewModels
{
    public class EstrusViewModel( IViewModelFactory viewModelFactory ):MvxViewModel
    {
        public SynchronizationListingViewModel? Synchronization => CreateViewModel();

        private SynchronizationListingViewModel? CreateViewModel()
        {
            SynchronizationListingViewModel? viewModel = viewModelFactory.CreateViewModel<SynchronizationListingViewModel>();
            viewModel?.Prepare();
            viewModel?.Initialize();
            return viewModel;
        }
    }
}
