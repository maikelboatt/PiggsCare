using PiggsCare.DataAccess.Repositories;
using PiggsCare.Domain.Models;

namespace PiggsCare.Domain.Services
{
    public class WeaningService( IWeaningRepository weaningRepository ):IWeaningService
    {
        public async Task<IEnumerable<WeaningEvent>> GetAllWeaningEventAsync( int id )
        {
            return await weaningRepository.GetAllWeaningEventAsync(id);
        }

        public async Task<WeaningEvent?> GetWeaningEventByIdAsync( int id )
        {
            return await weaningRepository.GetWeaningEventByIdAsync(id);
        }

        public async Task CreateWeaningEventAsync( WeaningEvent weaning )
        {
            await weaningRepository.CreateWeaningEventAsync(weaning);
        }

        public async Task UpdateWeaningEventAsync( WeaningEvent weaning )
        {
            await weaningRepository.UpdateWeaningEventAsync(weaning);
        }

        public async Task DeleteWeaningEventAsync( int id )
        {
            await weaningRepository.DeleteWeaningEventAsync(id);
        }
    }
}
