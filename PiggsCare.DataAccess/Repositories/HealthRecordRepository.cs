using PiggsCare.DataAccess.DatabaseAccess;
using PiggsCare.DataAccess.DTO;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Repositories;
using PiggsCare.Domain.Services;

namespace PiggsCare.DataAccess.Repositories
{
    public class HealthRecordRepository( ISqlDataAccess dataAccess, IDateConverterService dateConverterService ):IHealthRecordRepository
    {
        private const string Connectionstring = @"Server=--THEBARON--\SQLEXPRESS;Database=PiggyKare;Integrated Security=True;TrustServerCertificate=True;";

        public async Task<IEnumerable<HealthRecord>> GetAllHealthRecordsAsync( int id )
        {
            IEnumerable<HealthRecordDto> result = await dataAccess.QueryAsync<HealthRecordDto, dynamic>("dbo.GetAllHealthRecordsForAnimal", new { AnimalID = id }, Connectionstring);
            return result.Select(x => new HealthRecord(x.HealthRecordId, x.AnimalId, dateConverterService.GetDateOnly(x.RecordDate), x.Diagnosis, x.Treatment, x.Outcome));
        }

        public async Task<HealthRecord?> GetHealthRecordByIdAsync( int id )
        {
            // Query the database for any record with matching id
            IEnumerable<HealthRecordDto> result = await dataAccess.QueryAsync<HealthRecordDto, dynamic>("dbo.GetHealthRecordById", new { HealthRecordId = id }, Connectionstring);
            HealthRecordDto? healthRecordDto = result.FirstOrDefault();

            // Convert HealthRecordDto to HealthRecord Object
            return healthRecordDto is not null
                ? new HealthRecord(healthRecordDto.HealthRecordId,
                                   healthRecordDto.AnimalId,
                                   dateConverterService.GetDateOnly(healthRecordDto.RecordDate),
                                   healthRecordDto.Diagnosis,
                                   healthRecordDto.Treatment,
                                   healthRecordDto.Outcome)
                : null;
        }

        public async Task CreateHealthRecordAsync( HealthRecord health )
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
            await dataAccess.CommandAsync("dbo.InsertHealthRecord",
                                          new
                                          {
                                              record.AnimalId, record.RecordDate, record.Diagnosis, record.Treatment, record.Outcome
                                          },
                                          Connectionstring
                );
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
            await dataAccess.CommandAsync("dbo.UpdateHealthRecord", recordDto, Connectionstring);
        }

        public async Task DeleteHealthRecordAsync( int id )
        {
            await dataAccess.CommandAsync("dbo.DeleteHealthRecord", new { HealthRecordId = id }, Connectionstring);
        }
    }
}
