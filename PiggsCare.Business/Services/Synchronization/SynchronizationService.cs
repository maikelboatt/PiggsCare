using PiggsCare.ApplicationState.Stores.Synchronization;
using PiggsCare.Business.Services.Errors;
using PiggsCare.DataAccess.Repositories.Synchronization;
using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.Synchronization
{
    /// <summary>
    ///     Service for managing synchronization events.
    /// </summary>
    /// <param name="synchronizationEventRepository" >Repository for synchronization events.</param>
    public class SynchronizationService(
        ISynchronizationEventRepository synchronizationEventRepository,
        ISynchronizationEventStore synchronizationEventStore,
        IDatabaseErrorHandlerService databaseErrorHandlerService ):ISynchronizationService
    {
        /// <summary>
        ///     Retrieves all synchronization events asynchronously.
        /// </summary>
        /// <returns>A collection of <see cref="SynchronizationEvent" />.</returns>
        public async Task GetAllSynchronizationEventsAsync()
        {
            IEnumerable<SynchronizationEvent> synchronizations = await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                synchronizationEventRepository.GetAllSynchronizationEventsAsync,
                "Retrieving all synchronizations"
            ) ?? [];
            synchronizationEventStore.LoadSynchronizationEvents(synchronizations);
        }

        /// <summary>
        ///     Retrieves a synchronization event by its ID asynchronously.
        /// </summary>
        /// <param name="synchronizationEventId" >The ID of the synchronization event.</param>
        /// <returns>The <see cref="SynchronizationEvent" /> if found; otherwise, null.</returns>
        public SynchronizationEvent? GetSynchronizationEventByIdAsync( int synchronizationEventId ) => synchronizationEventStore.GetSynchronizationEventById(synchronizationEventId);

        /// <summary>
        ///     Creates a new synchronization event asynchronously.
        /// </summary>
        /// <param name="synchronizationEvent" >The synchronization event to create.</param>
        /// <returns>The ID of the created synchronization event.</returns>
        public async Task<int> CreateSynchronizationEventAsync( SynchronizationEvent synchronizationEvent )
        {
            int? uniqueId = await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => synchronizationEventRepository.CreateSynchronizationEventAsync(synchronizationEvent),
                "Adding synchronizations"
            );
            {
                SynchronizationEvent synchronizationEventWithCorrectId = synchronizationEventStore.CreateSynchronizationEventWithCorrectId(uniqueId.Value, synchronizationEvent);
                synchronizationEventStore.AddSynchronizationEvent(synchronizationEventWithCorrectId);
            }
            return uniqueId.Value;
        }

        /// <summary>
        ///     Updates a synchronization event asynchronously.
        /// </summary>
        /// <param name="synchronizationEvent" >The synchronization event to update.</param>
        public async Task UpdateSynchronizationEventAsync( SynchronizationEvent synchronizationEvent )
        {
            await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => synchronizationEventRepository.UpdateSynchronizationEventAsync(synchronizationEvent),
                "Updating synchronizations"
            );
            synchronizationEventStore.UpdateSynchronizationEvent(synchronizationEvent);
        }

        /// <summary>
        ///     Deletes a synchronization event by its ID asynchronously.
        /// </summary>
        /// <param name="synchronizationEventId" >The ID of the synchronization event to delete.</param>
        public async Task DeleteSynchronizationEventAsync( int synchronizationEventId )
        {
            await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => synchronizationEventRepository.DeleteSynchronizationEventAsync(synchronizationEventId),
                "Deleting synchronizations"
            );
            synchronizationEventStore.DeleteSynchronizationEvent(synchronizationEventId);
        }
    }
}
