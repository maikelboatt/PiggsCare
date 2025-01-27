using MvvmCross.ViewModels;
using PiggsCare.Core.Factory;

namespace PiggsCare.Core.ViewModels
{
    public class HomeViewModel( IViewModelFactory viewModelFactory ):MvxViewModel, IHomeViewModel
    {
        public AnimalListingViewModel? AnimalListingViewModel => CreateViewModel();

        public override Task Initialize()
        {
            Console.WriteLine("Initialize HomeViewModel");
            return base.Initialize();
        }

        private AnimalListingViewModel? CreateViewModel()
        {
            AnimalListingViewModel? viewmodel = viewModelFactory.CreateViewModel<AnimalListingViewModel>();
            viewmodel?.Initialize();
            return viewmodel;
        }
    }
}
