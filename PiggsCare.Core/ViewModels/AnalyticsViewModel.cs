using MvvmCross.ViewModels;
using PiggsCare.Core.Factory;

namespace PiggsCare.Core.ViewModels
{
    public class AnalyticsViewModel( IViewModelFactory viewModelFactory ):MvxViewModel
    {
        public DashboardViewModel? Dashboard => CreateViewModel();

        private DashboardViewModel? CreateViewModel()
        {
            DashboardViewModel? viewModel = viewModelFactory.CreateViewModel<DashboardViewModel>();
            viewModel?.Prepare();
            viewModel?.Initialize();
            return viewModel;
        }
    }
}
