using PiggsCare.DataAccess.DatabaseAccess;
using PiggsCare.DataAccess.DTO;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Repositories;
using PiggsCare.Domain.Services;

namespace PiggsCare.DataAccess.Repositories
{
    public class BreedingEventRepository( ISqlDataAccess dataAccess, IDateConverterService dateConverterService ):IBreedingEventRepository
    {
        private const string Connectionstring = @"Server=--THEBARON--\SQLEXPRESS;Database=PiggsCare;Integrated Security=True;TrustServerCertificate=True;";

        public async Task<IEnumerable<BreedingEvent>> GetAllBreedingEventsAsync( int id )
        {
            // Query the database for all breeding events of animal with specified id
            IEnumerable<BreedingEventDto> result = await dataAccess.QueryAsync<BreedingEventDto, dynamic>("sp.Insemination_GetAll", new { AnimalId = id }, Connectionstring);

            // Returns a BreedingEventDto converted into a BreedingEvent Object
            return result.Select(x => new BreedingEvent(x.BreedingEventId,
                                                        x.AnimalId,
                                                        dateConverterService.GetDateOnly(x.AiDate),
                                                        dateConverterService.GetDateOnly(x.ExpectedFarrowDate),
                                                        x.SynchronizationEventId));
        }

        public async Task<IEnumerable<BreedingEventWithAnimal>> GetAllBreedingEventBySynchronizationBatchAsync( int synchronizationId )
        {
            // Query the database for all breeding events of synchronization batch with specified id
            IEnumerable<BreedingEventWithAnimalDto> result =
                await dataAccess.QueryAsync<BreedingEventWithAnimalDto, dynamic>("sp.Insemination_GetAllInBatch", new { SynchronizationEventId = synchronizationId }, Connectionstring);

            // Returns a BreedingEventWithAnimalDto converted into a BreedingEventWithAnimal Object
            return result.Select(x => new BreedingEventWithAnimal(x.BreedingEventId,
                                                                  x.AnimalId,
                                                                  dateConverterService.GetDateOnly(x.AiDate),
                                                                  dateConverterService.GetDateOnly(x.ExpectedFarrowDate),
                                                                  x.SynchronizationEventId,
                                                                  x.Name));
        }

        public async Task<BreedingEvent?> GetBreedingEventByIdAsync( int id )
        {
            // Query the database for any breeding event record with matching id
            IEnumerable<BreedingEventDto> result = await dataAccess.QueryAsync<BreedingEventDto, dynamic>("sp.Insemination_GetUnique", new { BreedingEventId = id }, Connectionstring);
            BreedingEventDto? breedingEventDto = result.FirstOrDefault();

            // Convert BreedingEventDto to BreedingEvent Object
            return breedingEventDto is not null
                ? new BreedingEvent(breedingEventDto.BreedingEventId,
                                    breedingEventDto.AnimalId,
                                    dateConverterService.GetDateOnly(breedingEventDto.AiDate),
                                    dateConverterService.GetDateOnly(breedingEventDto.ExpectedFarrowDate),
                                    breedingEventDto.SynchronizationEventId)
                : null;
        }

        public async Task CreateBreedingEventAsync( BreedingEvent breeding )
        {
            // Convert BreedingEvent to BreedingEventDto
            BreedingEventDto record = new()
            {
                AnimalId = breeding.AnimalId,
                AiDate = dateConverterService.GetDateTime(breeding.AiDate),
                ExpectedFarrowDate = dateConverterService.GetDateTime(breeding.ExpectedFarrowDate),
                SynchronizationEventId = breeding.SynchronizationEventId
            };

            // Insert record into the database
            await dataAccess.CommandAsync("sp.Insemination_Insert", new { record.AnimalId, record.AiDate, record.ExpectedFarrowDate, record.SynchronizationEventId }, Connectionstring);
        }

        public async Task UpdateBreedingEventAsync( BreedingEvent breeding )
        {
            // Convert BreedingEvent to BreedingEventDto
            BreedingEventDto record = new()
            {
                BreedingEventId = breeding.BreedingEventId,
                AnimalId = breeding.AnimalId,
                AiDate = dateConverterService.GetDateTime(breeding.AiDate),
                ExpectedFarrowDate = dateConverterService.GetDateTime(breeding.ExpectedFarrowDate),
                SynchronizationEventId = breeding.SynchronizationEventId
            };

            // Update existing record in the database
            await dataAccess.CommandAsync("sp.Insemination_Modify", record, Connectionstring);
        }

        public async Task DeleteBreedingEventAsync( int id )
        {
            await dataAccess.CommandAsync("sp.Insemination_Delete", new { BreedingEventId = id }, Connectionstring);
        }
    }
}
