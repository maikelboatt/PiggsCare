using PiggsCare.Domain.Models;
using PiggsCare.Domain.Repositories;

namespace PiggsCare.Domain.Services
{
    public class AnimalService( IAnimalRepository animalRepository ):IAnimalService
    {
        public async Task<IEnumerable<Animal>> GetAllAnimalsAsync()
        {
            return await animalRepository.GetAllAnimalsAsync();
        }

        public async Task<Animal> GetAnimalByIdAsync( int id )
        {
            return await animalRepository.GetAnimalByIdAsync(id);
        }

        public async Task<Animal> CreateAnimalAsync( Animal animal )
        {
            return await animalRepository.CreateAnimalAsync(animal);
        }

        public async Task<Animal> UpdateAnimalAsync( Animal animal )
        {
            return await animalRepository.UpdateAnimalAsync(animal);
        }

        public async Task DeleteAnimalAsync( int id )
        {
            await animalRepository.DeleteAnimalAsync(id);
        }

        public async Task<Animal> GetAnimalByNameAsync( int name )
        {
            return await animalRepository.GetAnimalByNameAsync(name);
        }

        public async Task<IEnumerable<Animal>> GetAnimalByBreedAsync( string breed )
        {
            return await animalRepository.GetAnimalByBreedAsync(breed);
        }
    }
}
