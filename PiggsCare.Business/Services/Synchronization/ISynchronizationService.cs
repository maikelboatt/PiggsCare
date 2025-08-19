using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.Synchronization
{
    public interface ISynchronizationService
    {
        /// <summary>
        ///     Retrieves all synchronization events asynchronously.
        /// </summary>
        /// <returns>A collection of <see cref="SynchronizationEvent" />.</returns>
        Task GetAllSynchronizationEventsAsync();

        /// <summary>
        ///     Retrieves a synchronization event by its ID asynchronously.
        /// </summary>
        /// <param name="synchronizationEventId" >The ID of the synchronization event.</param>
        /// <returns>The <see cref="SynchronizationEvent" /> if found; otherwise, null.</returns>
        SynchronizationEvent? GetSynchronizationEventByIdAsync( int synchronizationEventId );

        /// <summary>
        ///     Updates a synchronization event asynchronously.
        /// </summary>
        /// <param name="synchronizationEvent" >The synchronization event to update.</param>
        Task UpdateSynchronizationEventAsync( SynchronizationEvent synchronizationEvent );

        /// <summary>
        ///     Deletes a synchronization event by its ID asynchronously.
        /// </summary>
        /// <param name="synchronizationEventId" >The ID of the synchronization event to delete.</param>
        Task DeleteSynchronizationEventAsync( int synchronizationEventId );

        /// <summary>
        ///     Creates a new synchronization event asynchronously.
        /// </summary>
        /// <param name="synchronizationEvent" >The synchronization event to create.</param>
        /// <returns>The ID of the created synchronization event.</returns>
        Task<int> CreateSynchronizationEventAsync( SynchronizationEvent synchronizationEvent );
    }
}
