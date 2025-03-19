using PiggsCare.DataAccess.Repositories;
using PiggsCare.Domain.Models;

namespace PiggsCare.Domain.Services
{
    public class FarrowingService( IFarrowRepository farrowRepository ):IFarrowingService
    {
        public async Task<IEnumerable<FarrowEvent>> GetAllFarrowEventAsync( int id )
        {
            return await farrowRepository.GetAllFarrowEventAsync(id);
        }

        public async Task<FarrowEvent?> GetFarrowEventByIdAsync( int id )
        {
            return await farrowRepository.GetFarrowEventByIdAsync(id);
        }

        public async Task CreateFarrowEventAsync( FarrowEvent farrow )
        {
            await farrowRepository.CreateFarrowEventAsync(farrow);
        }

        public async Task UpdateFarrowEventAsync( FarrowEvent farrow )
        {
            await farrowRepository.UpdateFarrowEventAsync(farrow);
        }

        public async Task DeleteFarrowEventAsync( int id )
        {
            await farrowRepository.DeleteFarrowEventAsync(id);
        }
    }
}
