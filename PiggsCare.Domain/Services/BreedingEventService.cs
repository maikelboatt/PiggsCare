using PiggsCare.Domain.Models;
using PiggsCare.Domain.Repositories;

namespace PiggsCare.Domain.Services
{
    public class BreedingEventService( IBreedingEventRepository breedingEventRepository ):IBreedingEventService
    {
        public async Task<IEnumerable<BreedingEvent>> GetAllBreedingEventsAsync( int id )
        {
            return await breedingEventRepository.GetAllBreedingEventsAsync(id);
        }

        public async Task<BreedingEvent?> GetBreedingEventByIdAsync( int id )
        {
            return await breedingEventRepository.GetBreedingEventByIdAsync(id);
        }

        public async Task CreateBreedingEventAsync( BreedingEvent breeding )
        {
            await breedingEventRepository.CreateBreedingEventAsync(breeding);
        }

        public async Task UpdateBreedingEventAsync( BreedingEvent breeding )
        {
            await breedingEventRepository.UpdateBreedingEventAsync(breeding);
        }

        public async Task DeleteBreedingEventAsync( int id )
        {
            await breedingEventRepository.DeleteBreedingEventAsync(id);
        }
    }
}
