using PiggsCare.Domain.Models;
using PiggsCare.Domain.Repositories;

namespace PiggsCare.Domain.Services
{
    public class PregnancyService( IPregnancyRepository pregnancyRepository ):IPregnancyService
    {
        public async Task<IEnumerable<PregnancyScan>> GetAllPregnancyScansAsync( int id )
        {
            return await pregnancyRepository.GetAllPregnancyScansAsync(id);
        }

        public async Task<PregnancyScan?> GetPregnancyScanByIdAsync( int id )
        {
            return await pregnancyRepository.GetPregnancyScanByIdAsync(id);
        }

        public async Task CreatePregnancyScanAsync( PregnancyScan scan )
        {
            await pregnancyRepository.CreatePregnancyScanAsync(scan);
        }

        public async Task UpdatePregnancyScanAsync( PregnancyScan scan )
        {
            await pregnancyRepository.UpdatePregnancyScanAsync(scan);
        }

        public async Task DeleteHealthRecordAsync( int id )
        {
            await pregnancyRepository.DeletePregnancyScanAsync(id);
        }
    }
}
