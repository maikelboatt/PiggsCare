using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.Insemination
{
    public interface IInseminationService
    {
        /// <summary>
        ///     Retrieves all insemination events for a given id.
        /// </summary>
        /// <param name="id" >The identifier for which to retrieve insemination events.</param>
        /// <returns>A collection of <see cref="InseminationEvent" />.</returns>
        Task GetAllInseminationEventsAsync( int id );

        /// <summary>
        ///     Retrieves all insemination events with animal details by synchronization batch.
        /// </summary>
        /// <param name="synchronizationId" >The synchronization batch identifier.</param>
        /// <returns>A collection of <see cref="InseminationEventWithAnimal" />.</returns>
        void GetAllInseminationEventBySynchronizationBatchAsync( int synchronizationId );

        /// <summary>
        ///     Retrieves a specific insemination event by its id.
        /// </summary>
        /// <param name="id" >The insemination event identifier.</param>
        /// <returns>The <see cref="InseminationEvent" /> if found; otherwise, null.</returns>
        InseminationEvent? GetInseminationEventByIdAsync( int id );

        InseminationEvent? GetInseminationEventBySynchronizationIdAsync( int synchronizationId );

        /// <summary>
        ///     Creates a new insemination event.
        /// </summary>
        /// <param name="insemination" >The insemination event to create.</param>
        Task CreateInseminationEventAsync( InseminationEvent insemination );

        /// <summary>
        ///     Updates an existing insemination event.
        /// </summary>
        /// <param name="insemination" >The insemination event to update.</param>
        Task UpdateInseminationEventAsync( InseminationEvent insemination );

        /// <summary>
        ///     Deletes an insemination event by its inseminationId.
        /// </summary>
        /// <param name="inseminationId" >The identifier of the insemination event to delete.</param>
        Task DeleteInseminationEventAsync( int inseminationId );
    }
}
