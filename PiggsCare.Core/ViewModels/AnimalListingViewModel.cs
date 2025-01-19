using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;

namespace PiggsCare.Core.ViewModels
{
    public class AnimalListingViewModel:MvxViewModel
    {
        private static IEnumerable<Animal> animalsEnumerable = [];
        private readonly MvxObservableCollection<Animal> _animals = new(animalsEnumerable);

        private readonly IAnimalService _animalService;
        // private readonly MvxSubscriptionToken _token;

        public AnimalListingViewModel( IAnimalService animalService, IMvxMessenger messenger )
        {
            _animalService = animalService;
            // _token = messenger.Subscribe<AnimalMessage>(AnimalListMessage);
        }

        public IEnumerable<Animal> Animals => _animals;

        public IMvxAsyncCommand LoadAnimalsCommand => new MvxAsyncCommand(LoadAnimalsAsyncExecute);

        // private void AnimalListMessage( AnimalMessage animals )
        // {
        //     animalsEnumerable = animals.Animals;
        // }

        public override void Prepare()
        {
            base.Prepare();
        }

        public override async Task Initialize()
        {
            await LoadAnimalsAsyncExecute();

            await base.Initialize();
        }

        private async Task LoadAnimalsAsyncExecute()
        {
            try
            {
                animalsEnumerable = await _animalService.GetAllAnimalsAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
