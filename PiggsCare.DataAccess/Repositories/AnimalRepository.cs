using PiggsCare.DataAccess.DatabaseAccess;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Repositories;

namespace PiggsCare.DataAccess.Repositories
{
    public class AnimalRepository( ISqlDataAccess dataAccess ):IAnimalRepository
    {
        private const string Connectionstring = @"Server=--THEBARON--\SQLEXPRESS;Database=PiggyKare;Integrated Security=True;TrustServerCertificate=True;";

        public async Task<IEnumerable<Animal>> GetAllAnimalsAsync()
        {
            return await dataAccess.QueryAsync<Animal, dynamic>("dbo.GetAllAnimals", new { }, Connectionstring);
        }

        public async Task<Animal?> GetAnimalByIdAsync( int id )
        {
            IEnumerable<Animal> result = await dataAccess.QueryAsync<Animal, dynamic>("dbo.GetAnimalById", new { AnimalID = id }, Connectionstring);
            return result.FirstOrDefault();
        }

        public async Task CreateAnimalAsync( Animal animal )
        {
            await dataAccess.CommandAsync("dbo.InsertAnimal",
                                          new
                                          {
                                              animal.Name, animal.Breed, animal.BirthDate, animal.CertificateNumber
                                          },
                                          Connectionstring);
        }

        public async Task UpdateAnimalAsync( Animal animal )
        {
            await dataAccess.CommandAsync("dbo.UpdateAnimal", animal, Connectionstring);
        }

        public async Task DeleteAnimalAsync( int id )
        {
            await dataAccess.CommandAsync("DeleteAnimal", new { Id = id });
        }

        public async Task<Animal?> GetAnimalByNameAsync( int name )
        {
            IEnumerable<Animal?> result = await dataAccess.QueryAsync<Animal, dynamic>("dbo.GetAnimalByName", new { Name = name }, Connectionstring);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Animal>> GetAnimalByBreedAsync( string breed )
        {
            return await dataAccess.QueryAsync<Animal, dynamic>("GetAnimalByBreed", new { Breed = breed });
        }
    }
}
