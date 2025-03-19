using PiggsCare.Domain.Models;
using PiggsCare.Domain.Repositories;

namespace PiggsCare.Domain.Services
{
    public class RemovalEventService( IRemovalEventRepository eventRepository ):IRemovalEventService
    {
        public async Task<IEnumerable<RemovalEvent>> GetAllRemovalEventsAsync( int animalId )
        {
            return await eventRepository.GetAllRemovalEventsAsync(animalId);
        }

        public async Task<RemovalEvent?> GetRemovalEventByIdAsync( int removalEventId )
        {
            return await eventRepository.GetRemovalEventByIdAsync(removalEventId);
        }

        public async Task CreateRemovalEventAsync( RemovalEvent removalEvent )
        {
            await eventRepository.CreateRemovalEventAsync(removalEvent);
        }

        public async Task UpdateRemovalEventAsync( RemovalEvent removalEvent )
        {
            await eventRepository.UpdateRemovalEventAsync(removalEvent);
        }

        public async Task DeleteRemovalEventAsync( int removalEventId )
        {
            await eventRepository.DeleteRemovalEventAsync(removalEventId);
        }
    }
}
