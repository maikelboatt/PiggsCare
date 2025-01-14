using Microsoft.Extensions.Configuration;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Repositories;

namespace PiggsCare.DataAccess.Repositories
{
    public class AnimalRepository(  ):IAnimalRepository
    {
        public Task<IEnumerable<Animal>> GetAllAnimalsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Animal> GetAnimalByIdAsync( int id )
        {
            throw new NotImplementedException();
        }

        public Task<Animal> CreateAnimalAsync( Animal animal )
        {
            throw new NotImplementedException();
        }

        public Task<Animal> UpdateAnimalAsync( Animal animal )
        {
            throw new NotImplementedException();
        }

        public Task DeleteAnimalAsync( int id )
        {
            throw new NotImplementedException();
        }

        public Task<Animal> GetAnimalByNameAsync( int name )
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Animal>> GetAnimalByBreedAsync( string breed )
        {
            throw new NotImplementedException();
        }
    }
}
