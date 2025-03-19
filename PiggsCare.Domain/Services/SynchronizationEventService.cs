using PiggsCare.Domain.Models;
using PiggsCare.Domain.Repositories;

namespace PiggsCare.Domain.Services
{
    public class SynchronizationEventService( ISynchronizationEventRepository eventRepository ):ISynchronizationEventService
    {
        public async Task<IEnumerable<SynchronizationEvent>> GetAllSynchronizationEventsAsync()
        {
            return await eventRepository.GetAllSynchronizationEventsAsync();
        }

        public async Task<SynchronizationEvent?> GetSynchronizationEventByIdAsync( int synchronizationEventId )
        {
            return await eventRepository.GetSynchronizationEventByIdAsync(synchronizationEventId);
        }

        public async Task UpdateSynchronizationEventAsync( SynchronizationEvent synchronizationEvent )
        {
            await eventRepository.UpdateSynchronizationEventAsync(synchronizationEvent);
        }

        public async Task DeleteSynchronizationEventAsync( int synchronizationEventId )
        {
            await eventRepository.DeleteSynchronizationEventAsync(synchronizationEventId);
        }

        public async Task<int> CreateSynchronizationEventAsync( SynchronizationEvent synchronizationEvent )
        {
            return await eventRepository.CreateSynchronizationEventAsync(synchronizationEvent);
        }
    }
}
