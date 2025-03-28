using PiggsCare.DataAccess.DatabaseAccess;
using PiggsCare.DataAccess.DTO;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Repositories;
using PiggsCare.Domain.Services;

namespace PiggsCare.DataAccess.Repositories
{
    public class RemovalEventRepository( ISqlDataAccess dataAccess, IDateConverterService dateConverterService ):IRemovalEventRepository
    {
        private const string Connectionstring = @"Server=--THEBARON--\SQLEXPRESS;Database=PiggsCare;Integrated Security=True;TrustServerCertificate=True;"; // Ideally from config

        public async Task<IEnumerable<RemovalEvent>> GetAllRemovalEventsAsync( int animalId )
        {
            IEnumerable<RemovalEventDto> results = await dataAccess.QueryAsync<RemovalEventDto, dynamic>(
                "sp.Removal_GetAll", // Stored procedure name for GetAll
                new { AnimalId = animalId },
                Connectionstring);

            return results.Select(x => new RemovalEvent(x.RemovalEventId,
                                                        x.AnimalId,
                                                        dateConverterService.GetDateOnly(x.RemovalDate),
                                                        x.ReasonForRemoval)); // Directly return DTOs, assuming service layer handles conversion if needed.
        }

        public async Task<RemovalEvent?> GetRemovalEventByIdAsync( int removalEventId )
        {
            IEnumerable<RemovalEventDto> results = await dataAccess.QueryAsync<RemovalEventDto, dynamic>(
                "sp.Removal_GetUnique", // Stored procedure name for GetById
                new { RemovalEventId = removalEventId },
                Connectionstring);

            RemovalEventDto? removalEventDto = results.FirstOrDefault();

            return removalEventDto is not null
                ? new RemovalEvent(removalEventDto.RemovalEventId,
                                   removalEventDto.AnimalId,
                                   dateConverterService.GetDateOnly(removalEventDto.RemovalDate),
                                   removalEventDto.ReasonForRemoval)
                : null;
        }

        public async Task CreateRemovalEventAsync( RemovalEvent removalEvent )
        {
            // Convert RemovalEvent to RemovalEventDto
            RemovalEventDto record = new()
            {
                AnimalId = removalEvent.AnimalId,
                RemovalDate = dateConverterService.GetDateTime(removalEvent.RemovalDate),
                ReasonForRemoval = removalEvent.ReasonForRemoval
            };

            // Insert record into the database
            await dataAccess.CommandAsync(
                "sp.Removal_Insert", // Stored procedure name for Insert
                new
                {
                    removalEvent.AnimalId,
                    RemovalDate = dateConverterService.GetDateTime(removalEvent.RemovalDate), // Convert DateOnly to DateTime for DB
                    removalEvent.ReasonForRemoval
                },
                Connectionstring);
        }

        public async Task UpdateRemovalEventAsync( RemovalEvent removalEvent )
        {
            // Convert RemovalEvent to RemovalEventDto
            RemovalEventDto record = new()
            {
                AnimalId = removalEvent.AnimalId,
                RemovalDate = dateConverterService.GetDateTime(removalEvent.RemovalDate),
                ReasonForRemoval = removalEvent.ReasonForRemoval
            };

            await dataAccess.CommandAsync(
                "sp.Removal_Modify", // Stored procedure name for Update
                new
                {
                    removalEvent.RemovalEventId,
                    removalEvent.AnimalId,
                    RemovalDate = dateConverterService.GetDateTime(removalEvent.RemovalDate), // Convert DateOnly to DateTime
                    removalEvent.ReasonForRemoval
                },
                Connectionstring);
        }

        public async Task DeleteRemovalEventAsync( int removalEventId )
        {
            await dataAccess.CommandAsync(
                "sp.Removal_Delete", // Stored procedure name for Delete
                new { RemovalEventId = removalEventId },
                Connectionstring);
        }
    }
}
