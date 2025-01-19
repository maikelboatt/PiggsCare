using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PiggsCare.Core.Commands;
using PiggsCare.Domain.Services;
using System.Windows.Input;

namespace PiggsCare.Core.ViewModels
{
    public class HomeViewModel:MvxViewModel
    {
        private readonly IAnimalService _animalService;
        private readonly IMvxMessenger _messenger;

        public HomeViewModel( IAnimalService animalService, IMvxNavigationService mvxNavigationService, IMvxMessenger messenger )
        {
            _animalService = animalService;
            _messenger = messenger;

            mvxNavigationService.Navigate<AnimalListingViewModel>();
        }

        // public ICommand LoadAnimalsCommand { get; set; }

        public AnimalListingViewModel AnimalListingViewModel => new(_animalService, _messenger);
    }
}
