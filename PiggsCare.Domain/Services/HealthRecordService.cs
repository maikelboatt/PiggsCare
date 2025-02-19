using PiggsCare.Domain.Models;
using PiggsCare.Domain.Repositories;

namespace PiggsCare.Domain.Services
{
    public class HealthRecordService( IHealthRecordRepository recordRepository ):IHealthRecordService
    {
        public async Task<IEnumerable<HealthRecord>> GetAllHealthRecordsAsync( int id )
        {
            return await recordRepository.GetAllHealthRecordsAsync(id);
        }

        public async Task<HealthRecord?> GetHealthRecordByIdAsync( int id )
        {
            return await recordRepository.GetHealthRecordByIdAsync(id);
        }

        public async Task CreateHealthRecordAsync( HealthRecord health )
        {
            await recordRepository.CreateHealthRecordAsync(health);
        }

        public async Task UpdateHealthRecordAsync( HealthRecord health )
        {
            await recordRepository.UpdateHealthRecordAsync(health);
        }

        public async Task DeleteHealthRecordAsync( int id )
        {
            await recordRepository.DeleteHealthRecordAsync(id);
        }
    }
}
