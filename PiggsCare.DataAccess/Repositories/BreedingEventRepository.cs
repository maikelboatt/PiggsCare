using PiggsCare.DataAccess.DatabaseAccess;
using PiggsCare.DataAccess.DTO;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Repositories;
using PiggsCare.Domain.Services;

namespace PiggsCare.DataAccess.Repositories
{
    public class BreedingEventRepository( ISqlDataAccess dataAccess, IDateConverterService dateConverterService ):IBreedingEventRepository
    {
        private const string Connectionstring = @"Server=--THEBARON--\SQLEXPRESS;Database=PiggyKare;Integrated Security=True;TrustServerCertificate=True;";

        public async Task<IEnumerable<BreedingEvent>> GetAllBreedingEventsAsync( int id )
        {
            // Query the database for all breeding events of animal with specified id
            IEnumerable<BreedingEventDto> result = await dataAccess.QueryAsync<BreedingEventDto, dynamic>("dbo.GetAllBreedingForAnimal", new { AnimalId = id }, Connectionstring);

            // Returns a BreedingEventDto converted into a BreedingEvent Object
            return result.Select(x => new BreedingEvent(x.BreedingEventId,
                                                        x.AnimalId,
                                                        dateConverterService.GetDateOnly(x.AiDate),
                                                        dateConverterService.GetDateOnly(x.ExpectedFarrowDate)));
        }

        public async Task<BreedingEvent?> GetBreedingEventByIdAsync( int id )
        {
            // Query the database for any breeding event record with matching id
            IEnumerable<BreedingEventDto> result = await dataAccess.QueryAsync<BreedingEventDto, dynamic>("dbo.GetBreedingById", new { BreedingEventId = id }, Connectionstring);
            BreedingEventDto? breedingEventDto = result.FirstOrDefault();

            // Convert BreedingEventDto to BreedingEvent Object
            return breedingEventDto is not null
                ? new BreedingEvent(breedingEventDto.BreedingEventId,
                                    breedingEventDto.AnimalId,
                                    dateConverterService.GetDateOnly(breedingEventDto.AiDate),
                                    dateConverterService.GetDateOnly(breedingEventDto.ExpectedFarrowDate))
                : null;
        }

        public async Task CreateBreedingEventAsync( BreedingEvent breeding )
        {
            // Convert BreedingEvent to BreedingEventDto
            BreedingEventDto record = new()
            {
                AnimalId = breeding.AnimalId,
                AiDate = dateConverterService.GetDateTime(breeding.AiDate),
                ExpectedFarrowDate = dateConverterService.GetDateTime(breeding.ExpectedFarrowDate)
            };

            // Insert record into the database
            await dataAccess.CommandAsync("dbo.InsertBreedingEvent", new { record.AnimalId, record.AiDate, record.ExpectedFarrowDate }, Connectionstring);
        }

        public async Task UpdateBreedingEventAsync( BreedingEvent breeding )
        {
            // Convert BreedingEvent to BreedingEventDto
            BreedingEventDto record = new()
            {
                BreedingEventId = breeding.BreedingEventId,
                AnimalId = breeding.AnimalId,
                AiDate = dateConverterService.GetDateTime(breeding.AiDate),
                ExpectedFarrowDate = dateConverterService.GetDateTime(breeding.ExpectedFarrowDate)
            };

            // Update existing record in the database
            await dataAccess.CommandAsync("dbo.UpdateBreedingEvent", record, Connectionstring);
        }

        public async Task DeleteBreedingEventAsync( int id )
        {
            await dataAccess.CommandAsync("dbo.DeleteBreedingEvent", new { BreedingEventId = id }, Connectionstring);
        }
    }
}
