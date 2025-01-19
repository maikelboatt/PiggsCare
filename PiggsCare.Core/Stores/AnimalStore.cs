using MvvmCross.Plugin.Messenger;
using PiggsCare.Core.Messages;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;

namespace PiggsCare.Core.Stores
{
    public class AnimalStore
    {
        private readonly IEnumerable<Animal> _animals =
        [
            new()
            {
                Name = 724,
                Breed = "null",
                BirthDate = DateTime.Now,
                CertificateNumber = 10
            }
        ];

        private readonly IAnimalService _animalService;
        private readonly IMvxMessenger _messenger;

        public AnimalStore( IAnimalService animalService, IMvxMessenger messenger )
        {
            _animalService = animalService;
            _messenger = messenger;
            // _animals = LoadAnimals().Result;
        }

        // private async Task<IEnumerable<Animal>> LoadAnimals()
        // {
        //     return await _animalService.GetAllAnimalsAsync();
        // }

        private void ShareAnimals( IEnumerable<Animal> animal )
        {
            _messenger.Publish(new AnimalMessage(this, _animals));
        }
    }
}
