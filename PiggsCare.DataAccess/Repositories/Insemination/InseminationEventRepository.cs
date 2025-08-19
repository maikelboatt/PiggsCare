using PiggsCare.DataAccess.DatabaseAccess;
using PiggsCare.DataAccess.DTO;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Services;

namespace PiggsCare.DataAccess.Repositories.Insemination
{
    public class InseminationEventRepository( ISqlDataAccess dataAccess, IDateConverterService dateConverterService ):IInseminationEventRepository
    {
        public async Task<IEnumerable<InseminationEvent>> GetAllInseminationEventsAsync( int id )
        {
            // Query the database for all insemination events of animal with specified id
            IEnumerable<InseminationEventDto> result = await dataAccess.QueryAsync<InseminationEventDto, dynamic>("sp.Insemination_GetAll", new { AnimalId = id });

            // Returns a InseminationEventDto converted into a InseminationEvent Object
            return result.Select(x => new InseminationEvent(
                                     x.BreedingEventId,
                                     x.AnimalId,
                                     dateConverterService.GetDateOnly(x.AiDate),
                                     dateConverterService.GetDateOnly(x.ExpectedFarrowDate),
                                     x.SynchronizationEventId));
        }

        public async Task<IEnumerable<InseminationEventWithAnimal>> GetAllInseminationEventsBySynchronizationBatchAsync( int synchronizationId )
        {
            // Query the database for all breeding events of synchronization batch with specified id
            IEnumerable<InseminationEventWithAnimalDto> result =
                await dataAccess.QueryAsync<InseminationEventWithAnimalDto, dynamic>("sp.Insemination_GetAllInBatch", new { SynchronizationEventId = synchronizationId });

            // Returns a BreedingEventWithAnimalDto converted into a BreedingEventWithAnimal Object
            return result.Select(x => new InseminationEventWithAnimal(
                                     x.BreedingEventId,
                                     x.AnimalId,
                                     dateConverterService.GetDateOnly(x.AiDate),
                                     dateConverterService.GetDateOnly(x.ExpectedFarrowDate),
                                     x.SynchronizationEventId,
                                     x.Name));
        }

        public async Task<InseminationEvent?> GetInseminationEventByIdAsync( int id )
        {
            // Query the database for any insemination event record with matching id
            IEnumerable<InseminationEventDto> result = await dataAccess.QueryAsync<InseminationEventDto, dynamic>("sp.Insemination_GetUnique", new { BreedingEventId = id });
            InseminationEventDto? breedingEventDto = result.FirstOrDefault();

            // Convert InseminationEventDto to InseminationEvent Object
            return breedingEventDto is not null
                ? new InseminationEvent(
                    breedingEventDto.BreedingEventId,
                    breedingEventDto.AnimalId,
                    dateConverterService.GetDateOnly(breedingEventDto.AiDate),
                    dateConverterService.GetDateOnly(breedingEventDto.ExpectedFarrowDate),
                    breedingEventDto.SynchronizationEventId)
                : null;
        }

        public async Task UpdateInseminationEventAsync( InseminationEvent insemination )
        {
            // Convert InseminationEvent to InseminationEventDto
            InseminationEventDto record = new()
            {
                BreedingEventId = insemination.BreedingEventId,
                AnimalId = insemination.AnimalId,
                AiDate = dateConverterService.GetDateTime(insemination.AiDate),
                ExpectedFarrowDate = dateConverterService.GetDateTime(insemination.ExpectedFarrowDate),
                SynchronizationEventId = insemination.SynchronizationEventId
            };

            // Update existing record in the database
            await dataAccess.CommandAsync("sp.Insemination_Modify", record);
        }

        public async Task DeleteInseminationEventAsync( int id )
        {
            await dataAccess.CommandAsync("sp.Insemination_Delete", new { BreedingEventId = id });
        }

        public async Task<int> CreateInseminationEventAsync( InseminationEvent insemination )
        {
            // Convert InseminationEvent to InseminationEventDto
            InseminationEventDto record = new()
            {
                AnimalId = insemination.AnimalId,
                AiDate = dateConverterService.GetDateTime(insemination.AiDate),
                ExpectedFarrowDate = dateConverterService.GetDateTime(insemination.ExpectedFarrowDate),
                SynchronizationEventId = insemination.SynchronizationEventId
            };

            // Insert record into the database
            IEnumerable<int> result = await dataAccess.QueryAsync<int, dynamic>(
                "sp.Insemination_Insert",
                new { record.AnimalId, record.AiDate, record.ExpectedFarrowDate, record.SynchronizationEventId });

            return result.Single();
        }

        public async Task<IEnumerable<InseminationEventWithAnimal>> GetAllInseminationEventBySynchronizationBatchAsync( int synchronizationId )
        {
            // Query the database for all insemination events of synchronization batch with specified id
            IEnumerable<InseminationEventWithAnimalDto> result =
                await dataAccess.QueryAsync<InseminationEventWithAnimalDto, dynamic>("sp.Insemination_GetAllInBatch", new { SynchronizationEventId = synchronizationId });

            // Returns a InseminationEventWithAnimalDto converted into a InseminationEventWithAnimal Object
            return result.Select(x => new InseminationEventWithAnimal(
                                     x.BreedingEventId,
                                     x.AnimalId,
                                     dateConverterService.GetDateOnly(x.AiDate),
                                     dateConverterService.GetDateOnly(x.ExpectedFarrowDate),
                                     x.SynchronizationEventId,
                                     x.Name));
        }
    }
}
