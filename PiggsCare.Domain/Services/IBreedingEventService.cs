using PiggsCare.Domain.Models;

namespace PiggsCare.Domain.Services
{
    public interface IBreedingEventService
    {
        /// <summary>
        ///     Method to Get all BreedingEvents in the database
        /// </summary>
        /// <param name="id" >Unique identification of animal</param>
        /// <returns>All breeding events in the database</returns>
        Task<IEnumerable<BreedingEvent>> GetAllBreedingEventsAsync( int id );

        /// <summary>
        ///     Method to get all breeding events by synchronization batch
        /// </summary>
        /// <param name="synchronizationId" >Unique identification of synchronization batch</param>
        /// <returns>All breeding events associated with the synchronization batch</returns>
        Task<IEnumerable<BreedingEventWithAnimal>> GetAllBreedingEventBySynchronizationBatchAsync( int synchronizationId );

        /// <summary>
        ///     Method that returns breeding event that has a unique id
        /// </summary>
        /// <param name="id" >Unique identification of breeding event </param>
        /// <returns>Returns breeding event with said id otherwise null</returns>
        Task<BreedingEvent?> GetBreedingEventByIdAsync( int id );

        /// <summary>
        ///     Creates a new breeding event
        /// </summary>
        /// <param name="breeding" > Details of the breeding event</param>
        /// <returns>the created breeding event</returns>
        Task CreateBreedingEventAsync( BreedingEvent breeding );

        /// <summary>
        ///     Updates an existing record
        /// </summary>
        /// <param name="breeding" >Change details</param>
        Task UpdateBreedingEventAsync( BreedingEvent breeding );

        /// <summary>
        ///     Deletes  breeding event from the database
        /// </summary>
        /// <param name="id" >Unique identification of breeding event</param>
        Task DeleteBreedingEventAsync( int id );
    }
}
