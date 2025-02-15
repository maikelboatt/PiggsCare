using PiggsCare.DataAccess.DatabaseAccess;
using PiggsCare.DataAccess.DTO;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Repositories;

namespace PiggsCare.DataAccess.Repositories
{
    public class HealthRecordRepository( ISqlDataAccess sqlDataAccess ):IHealthRecordRepository
    {
        private const string Connectionstring = @"Server=--THEBARON--\SQLEXPRESS;Database=PiggyKare;Integrated Security=True;TrustServerCertificate=True;";

        public async Task<IEnumerable<HealthRecord>> GetAllHealthRecordsAsync( int id )
        {
            IEnumerable<HealthRecordDto> result = await sqlDataAccess.QueryAsync<HealthRecordDto, dynamic>("dbo.GetAllHealthRecordsForAnimal", new { AnimalID = id }, Connectionstring);
            return result.Select(x => new HealthRecord(x.HealthRecordId, x.AnimalId, x.RecordDate, x.Diagnosis, x.Treatment, x.Outcome));
        }

        public Task<HealthRecord?> GetHealthRecordByIdAsync( int id )
        {
            throw new NotImplementedException();
        }

        public async Task CreateHealthRecordAsync( HealthRecord health )
        {
            // Convert HealthRecord to HealthRecordDto
            HealthRecordDto record = new()
            {
                AnimalId = health.AnimalId,
                RecordDate = health.RecordDate,
                Diagnosis = health.Diagnosis,
                Treatment = health.Treatment,
                Outcome = health.Outcome
            };

            // Insert record into the database
            await sqlDataAccess.CommandAsync("dbo.InsertHealthRecord",
                                             new
                                             {
                                                 record.AnimalId, record.RecordDate, record.Diagnosis, record.Treatment, record.Outcome
                                             },
                                             Connectionstring
                );
        }

        public Task UpdateHealthRecordAsync( HealthRecord health )
        {
            throw new NotImplementedException();
        }

        public Task DeleteHealthRecordAsync( int id )
        {
            throw new NotImplementedException();
        }

        public Task<HealthRecord?> GetHealthRecordByNameAsync( int name )
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HealthRecord>> GetHealthRecordByBreedAsync( string breed )
        {
            throw new NotImplementedException();
        }
    }
}
