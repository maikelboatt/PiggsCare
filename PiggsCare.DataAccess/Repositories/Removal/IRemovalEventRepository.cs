using PiggsCare.Domain.Models;

namespace PiggsCare.DataAccess.Repositories.Removal
{
    public interface IRemovalEventRepository
    {
        /// <summary>
        ///     Retrieves all removal events associated with a specific animal.
        /// </summary>
        /// <param name="animalId" >The unique identifier of the animal.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains an IEnumerable of RemovalEvent objects,
        ///     representing all removal events for the specified animal.
        /// </returns>
        Task<IEnumerable<RemovalEvent>> GetAllRemovalEventsAsync( int animalId );

        /// <summary>
        ///     Retrieves a removal event by its unique identifier.
        /// </summary>
        /// <param name="removalEventId" >The unique identifier of the removal event to retrieve.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains the RemovalEvent object if found, otherwise null.
        /// </returns>
        Task<RemovalEvent?> GetRemovalEventByIdAsync( int removalEventId );

        /// <summary>
        ///     Creates a new removal event in the database.
        /// </summary>
        /// <param name="removalEvent" >The RemovalEvent object containing the details of the new removal event.</param>
        /// <returns>A task that represents the asynchronous creation operation.</returns>
        Task<int> CreateRemovalEventAsync( RemovalEvent removalEvent );

        /// <summary>
        ///     Updates an existing removal event in the database.
        /// </summary>
        /// <param name="removalEvent" >The RemovalEvent object containing the updated details. The RemovalEventId must be valid.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        Task UpdateRemovalEventAsync( RemovalEvent removalEvent );

        /// <summary>
        ///     Deletes a removal event from the database based on its unique identifier.
        /// </summary>
        /// <param name="removalEventId" >The unique identifier of the removal event to delete.</param>
        /// <returns>A task that represents the asynchronous deletion operation.</returns>
        Task DeleteRemovalEventAsync( int removalEventId );
    }
}
