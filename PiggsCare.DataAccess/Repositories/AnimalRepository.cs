using PiggsCare.DataAccess.DatabaseAccess;
using PiggsCare.DataAccess.DTO;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Repositories;
using PiggsCare.Domain.Services;

namespace PiggsCare.DataAccess.Repositories
{
    public class AnimalRepository( ISqlDataAccess dataAccess, IDateConverterService dateConverterService ):IAnimalRepository
    {
        // private const string Connectionstring = @"Server=--THEBARON--\BOATT;Database=PiggsCare;User ID=PiggsCare_Admin;Password=1Passcode.;TrustServerCertificate=True;";
        //private const string Connectionstring = @"Server=--THEBARON--\BOATT;Database=PiggsCare;User ID=PiggsCare_Admin;Password=Merlin;TrustServerCertificate=True;";

        public async Task<IEnumerable<Animal>> GetAllAnimalsAsync()
        {
            // Query the database for all animal record in the database
            IEnumerable<AnimalDto> result = await dataAccess.QueryAsync<AnimalDto, dynamic>("sp.Animal_GetAll", new { });

            // Convert AnimalDto Object to Animal Object
            return result.Select(x => new Animal(x.AnimalId, x.Name, x.Breed, dateConverterService.GetDateOnly(x.BirthDate), x.CertificateNumber, x.Gender, x.BackFatIndex));
        }

        public async Task<Animal?> GetAnimalByIdAsync( int id )
        {
            // Query the database for any record with matching id
            IEnumerable<AnimalDto> result = await dataAccess.QueryAsync<AnimalDto, dynamic>("sp.Animal_GetUnique", new { AnimalID = id });
            AnimalDto? animalDto = result.FirstOrDefault();

            // Converts AnimalDto to Animal object
            return animalDto != null
                ? new Animal(
                    animalDto.AnimalId,
                    animalDto.Name,
                    animalDto.Breed,
                    dateConverterService.GetDateOnly(animalDto.BirthDate),
                    animalDto.CertificateNumber,
                    animalDto.Gender,
                    animalDto.BackFatIndex)
                : null;
        }

        public async Task UpdateAnimalAsync( Animal animal )
        {
            // Convert Animal Object to an AnimalDto Object
            AnimalDto record = new()
            {
                AnimalId = animal.AnimalId,
                Name = animal.Name,
                Breed = animal.Breed,
                BirthDate = dateConverterService.GetDateTime(animal.BirthDate),
                CertificateNumber = animal.CertificateNumber,
                Gender = animal.Gender,
                BackFatIndex = animal.BackFatIndex
            };

            // Update existing record in the database
            await dataAccess.CommandAsync("sp.Animal_Modify", record);
        }

        public async Task DeleteAnimalAsync( int id )
        {
            await dataAccess.CommandAsync("sp.Animal_Delete", new { AnimalId = id });
        }

        public async Task<Animal?> GetAnimalByNameAsync( int name )
        {
            // Query the database for record with matching name
            IEnumerable<AnimalDto?> result = await dataAccess.QueryAsync<AnimalDto, dynamic>("dbo.GetAnimalByIdentification", new { Name = name });
            AnimalDto? animalDto = result.FirstOrDefault();

            // Converts AnimalDto to Animal object
            return animalDto != null
                ? new Animal(
                    animalDto.AnimalId,
                    animalDto.Name,
                    animalDto.Breed,
                    dateConverterService.GetDateOnly(animalDto.BirthDate),
                    animalDto.CertificateNumber,
                    animalDto.Gender,
                    animalDto.BackFatIndex)
                : null;
        }

        public async Task<IEnumerable<Animal>> GetAnimalByBreedAsync( string breed )
        {
            IEnumerable<AnimalDto> result = await dataAccess.QueryAsync<AnimalDto, dynamic>("GetAnimalByBreed", new { Breed = breed });

            // Convert AnimalDto Object to Animal Object
            return result.Select(x => new Animal(x.AnimalId, x.Name, x.Breed, dateConverterService.GetDateOnly(x.BirthDate), x.CertificateNumber, x.Gender, x.BackFatIndex));
        }

        public async Task<int> CreateAnimalAsync( Animal animal )
        {
            // Convert Animal Object to an AnimalDto Object
            AnimalDto record = new()
            {
                Name = animal.Name,
                Breed = animal.Breed,
                BirthDate = dateConverterService.GetDateTime(animal.BirthDate),
                CertificateNumber = animal.CertificateNumber,
                Gender = animal.Gender,
                BackFatIndex = animal.BackFatIndex
            };

            // Inserts record into the database
            IEnumerable<int> result = await dataAccess.QueryAsync<int, dynamic>(
                "sp.Animal_Insert",
                new
                {
                    record.Name,
                    record.Breed,
                    record.BirthDate,
                    record.CertificateNumber,
                    record.Gender,
                    record.BackFatIndex
                }
            );

            return result.Single();
        }
    }
}
