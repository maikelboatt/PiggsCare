using PiggsCare.DataAccess.DatabaseAccess;
using PiggsCare.DataAccess.DTO;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Repositories;
using PiggsCare.Domain.Services;

namespace PiggsCare.DataAccess.Repositories
{
    public class SynchronizationEventRepository( ISqlDataAccess dataAccess, IDateConverterService dateConverterService ):ISynchronizationEventRepository
    {
        private const string Connectionstring = @"Server=--THEBARON--\SQLEXPRESS;Database=PiggsCare;Integrated Security=True;TrustServerCertificate=True;"; // Ideally from config

        public async Task<IEnumerable<SynchronizationEvent>> GetAllSynchronizationEventsAsync()
        {
            IEnumerable<EstrusSynchronizationEventDto> results = await dataAccess.QueryAsync<EstrusSynchronizationEventDto, dynamic>( // Querying for SynchronizationEvent Model directly
                "sp.Synchronization_GetAll",
                new { },
                Connectionstring);

            return results.Select(x => new SynchronizationEvent(x.SynchronizationEventId,
                                                                dateConverterService.GetDateOnly(x.StartDate),
                                                                dateConverterService.GetDateOnly(x.EndDate),
                                                                x.BatchNumber,
                                                                x.SynchronizationProtocol,
                                                                x.Comments));
        }

        public async Task<SynchronizationEvent?> GetSynchronizationEventByIdAsync( int synchronizationEventId )
        {
            IEnumerable<EstrusSynchronizationEventDto> results = await dataAccess.QueryAsync<EstrusSynchronizationEventDto, dynamic>( // Querying for SynchronizationEvent Model
                "sp.Synchronization_GetUnique",
                new { SynchronizationEventId = synchronizationEventId },
                Connectionstring);

            EstrusSynchronizationEventDto? eventDto = results.FirstOrDefault();

            return eventDto is not null
                ? new SynchronizationEvent(eventDto.SynchronizationEventId,
                                           dateConverterService.GetDateOnly(eventDto.StartDate),
                                           dateConverterService.GetDateOnly(eventDto.EndDate),
                                           eventDto.BatchNumber,
                                           eventDto.SynchronizationProtocol,
                                           eventDto.Comments)
                : null;
        }

        public async Task UpdateSynchronizationEventAsync( SynchronizationEvent synchronizationEvent )
        {
            // Convert SynchronizationEvent to SynchronizationEventDto
            EstrusSynchronizationEventDto record = new()
            {
                SynchronizationEventId = synchronizationEvent.SynchronizationEventId,
                StartDate = dateConverterService.GetDateTime(synchronizationEvent.StartDate),
                EndDate = dateConverterService.GetDateTime(synchronizationEvent.EndDate),
                BatchNumber = synchronizationEvent.BatchNumber,
                SynchronizationProtocol = synchronizationEvent.SynchronizationProtocol,
                Comments = synchronizationEvent.Comments
            };

            // Insert record into the database
            await dataAccess.CommandAsync(
                "sp.Synchronization_Modify",
                new
                {
                    synchronizationEvent.SynchronizationEventId,
                    StartDate = dateConverterService.GetDateTime(synchronizationEvent.StartDate), // Convert DateOnly to DateTime
                    EndDate = dateConverterService.GetDateTime(synchronizationEvent.EndDate),     // Convert DateOnly to DateTime
                    synchronizationEvent.BatchNumber,
                    synchronizationEvent.SynchronizationProtocol,
                    synchronizationEvent.Comments
                },
                Connectionstring);
        }

        public async Task DeleteSynchronizationEventAsync( int synchronizationEventId )
        {
            await dataAccess.CommandAsync(
                "sp.Synchronization_Delete",
                new { SynchronizationEventId = synchronizationEventId },
                Connectionstring);
        }

        public async Task<int> CreateSynchronizationEventAsync( SynchronizationEvent synchronizationEvent )
        {
            // Convert SynchronizationEvent to SynchronizationEventDto
            EstrusSynchronizationEventDto record = new()
            {
                SynchronizationEventId = synchronizationEvent.SynchronizationEventId,
                StartDate = dateConverterService.GetDateTime(synchronizationEvent.StartDate),
                EndDate = dateConverterService.GetDateTime(synchronizationEvent.EndDate),
                BatchNumber = synchronizationEvent.BatchNumber,
                SynchronizationProtocol = synchronizationEvent.SynchronizationProtocol,
                Comments = synchronizationEvent.Comments
            };

            // Insert record into the database
            IEnumerable<int> result = await dataAccess.QueryAsync<int, dynamic>(
                "sp.Synchronization_Insert",
                new
                {
                    StartDate = dateConverterService.GetDateTime(synchronizationEvent.StartDate), // Convert DateOnly to DateTime
                    EndDate = dateConverterService.GetDateTime(synchronizationEvent.EndDate),     // Convert DateOnly to DateTime
                    synchronizationEvent.BatchNumber,
                    synchronizationEvent.SynchronizationProtocol,
                    synchronizationEvent.Comments
                },
                Connectionstring);

            return result.Single();
        }
    }
}
