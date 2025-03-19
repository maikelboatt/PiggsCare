using PiggsCare.Domain.Models;

namespace PiggsCare.Domain.Services
{
    public interface ISynchronizationEventService
    {
        /// <summary>
        ///     Retrieves all synchronization events associated with a specific animal.
        ///     <returns>
        ///         A task that represents the asynchronous operation.
        ///         The task result contains an IEnumerable of SynchronizationEvent objects,
        ///         representing all synchronization events for the specified animal.
        ///     </returns>
        Task<IEnumerable<SynchronizationEvent>> GetAllSynchronizationEventsAsync();

        /// <summary>
        ///     Retrieves a synchronization event by its unique identifier.
        /// </summary>
        /// <param name="synchronizationEventId" >The unique identifier of the synchronization event to retrieve.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains the SynchronizationEvent object if found, otherwise null.
        /// </returns>
        Task<SynchronizationEvent?> GetSynchronizationEventByIdAsync( int synchronizationEventId );

        /// <summary>
        ///     Creates a new synchronization event in the database.
        /// </summary>
        /// <param name="synchronizationEvent" >
        ///     The SynchronizationEvent object containing the details of the new synchronization
        ///     event.
        /// </param>
        /// <returns>A task that represents the asynchronous creation operation.</returns>
        Task<int> CreateSynchronizationEventAsync( SynchronizationEvent synchronizationEvent );

        /// <summary>
        ///     Updates an existing synchronization event in the database.
        /// </summary>
        /// <param name="synchronizationEvent" >
        ///     The SynchronizationEvent object containing the updated details. The
        ///     SynchronizationEventId must be valid.
        /// </param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        Task UpdateSynchronizationEventAsync( SynchronizationEvent synchronizationEvent );

        /// <summary>
        ///     Deletes a synchronization event from the database based on its unique identifier.
        /// </summary>
        /// <param name="synchronizationEventId" >The unique identifier of the synchronization event to delete.</param>
        /// <returns>A task that represents the asynchronous deletion operation.</returns>
        Task DeleteSynchronizationEventAsync( int synchronizationEventId );
    }
}
