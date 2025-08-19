using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.Removal
{
    public interface IRemovalService
    {
        /// <summary>
        ///     Retrieves all removal events for a specific animal.
        /// </summary>
        /// <param name="animalId" >The ID of the animal.</param>
        /// <returns>A collection of removal events.</returns>
        Task GetAllRemovalEventsAsync( int animalId );

        /// <summary>
        ///     Retrieves a removal event by its ID.
        /// </summary>
        /// <param name="removalEventId" >The ID of the removal event.</param>
        /// <returns>The removal event if found; otherwise, null.</returns>
        RemovalEvent? GetRemovalEventByIdAsync( int removalEventId );

        /// <summary>
        ///     Creates a new removal event.
        /// </summary>
        /// <param name="removalEvent" >The removal event to create.</param>
        Task CreateRemovalEventAsync( RemovalEvent removalEvent );

        /// <summary>
        ///     Updates an existing removal event.
        /// </summary>
        /// <param name="removalEvent" >The removal event to update.</param>
        Task UpdateRemovalEventAsync( RemovalEvent removalEvent );

        /// <summary>
        ///     Deletes a removal event by its ID.
        /// </summary>
        /// <param name="removalEventId" >The ID of the removal event to delete.</param>
        Task DeleteRemovalEventAsync( int removalEventId );
    }
}
