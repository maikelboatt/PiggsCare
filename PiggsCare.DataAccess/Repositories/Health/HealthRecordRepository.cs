using PiggsCare.DataAccess.DatabaseAccess;
using PiggsCare.DataAccess.DTO;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Services;

namespace PiggsCare.DataAccess.Repositories.Health
{
    public class HealthRecordRepository( ISqlDataAccess dataAccess, IDateConverterService dateConverterService ):IHealthRecordRepository
    {
        public async Task<IEnumerable<HealthRecord>> GetAllHealthRecordsAsync( int id )
        {
            IEnumerable<HealthRecordDto> result = await dataAccess.QueryAsync<HealthRecordDto, dynamic>("sp.HealthRecords_GetAll", new { AnimalID = id });
            return result.Select(x => new HealthRecord(x.HealthRecordId, x.AnimalId, dateConverterService.GetDateOnly(x.RecordDate), x.Diagnosis, x.Treatment, x.Outcome));
        }

        public async Task<HealthRecord?> GetHealthRecordByIdAsync( int id )
        {
            // Query the database for any record with matching id
            IEnumerable<HealthRecordDto> result = await dataAccess.QueryAsync<HealthRecordDto, dynamic>("sp.HealthRecords_GetUnique", new { HealthRecordId = id });
            HealthRecordDto? healthRecordDto = result.FirstOrDefault();

            // Convert HealthRecordDto to HealthRecord Object
            return healthRecordDto is not null
                ? new HealthRecord(
                    healthRecordDto.HealthRecordId,
                    healthRecordDto.AnimalId,
                    dateConverterService.GetDateOnly(healthRecordDto.RecordDate),
                    healthRecordDto.Diagnosis,
                    healthRecordDto.Treatment,
                    healthRecordDto.Outcome)
                : null;
        }

        public async Task<int> CreateHealthRecordAsync( HealthRecord health )
        {
            // Convert HealthRecord to HealthRecordDto
            HealthRecordDto record = new()
            {
                AnimalId = health.AnimalId,
                RecordDate = dateConverterService.GetDateTime(health.RecordDate),
                Diagnosis = health.Diagnosis,
                Treatment = health.Treatment,
                Outcome = health.Outcome
            };

            // Insert record into the database
            IEnumerable<int> result = await dataAccess.QueryAsync<int, dynamic>(
                "sp.HealthRecords_Insert",
                new
                {
                    record.AnimalId,
                    record.RecordDate,
                    record.Diagnosis,
                    record.Treatment,
                    record.Outcome
                }
            );

            return result.Single();
        }

        public async Task UpdateHealthRecordAsync( HealthRecord health )
        {
            // Convert HealthRecord Object to HealthRecordDto Object
            HealthRecordDto recordDto = new()
            {
                HealthRecordId = health.HealthRecordId,
                AnimalId = health.AnimalId,
                RecordDate = dateConverterService.GetDateTime(health.RecordDate),
                Diagnosis = health.Diagnosis,
                Treatment = health.Treatment,
                Outcome = health.Outcome
            };

            // Update existing record in the database
            await dataAccess.CommandAsync("sp.HealthRecords_Modify", recordDto);
        }

        public async Task DeleteHealthRecordAsync( int id )
        {
            await dataAccess.CommandAsync("sp.HealthRecords_Delete", new { HealthRecordId = id });
        }
    }
}
