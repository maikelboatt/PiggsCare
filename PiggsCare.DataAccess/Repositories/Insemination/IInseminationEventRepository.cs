using PiggsCare.Domain.Models;

namespace PiggsCare.DataAccess.Repositories.Insemination
{
    public interface IInseminationEventRepository
    {
        /// <summary>
        ///     Method to get all insemination events in the database
        /// </summary>
        /// <param name="id" >Unique identification of animal</param>
        /// <returns>All insemination events in the database</returns>
        Task<IEnumerable<InseminationEvent>> GetAllInseminationEventsAsync( int id );

        /// <summary>
        ///     Method to get all insemination events by synchronization batch
        /// </summary>
        /// <param name="synchronizationId" >Unique identification of synchronization batch</param>
        /// <returns>All insemination events associated with the synchronization batch</returns>
        Task<IEnumerable<InseminationEventWithAnimal>> GetAllInseminationEventsBySynchronizationBatchAsync( int synchronizationId );

        /// <summary>
        ///     Method that returns insemination event that has a unique id
        /// </summary>
        /// <param name="id" >Unique identification of insemination event </param>
        /// <returns>Returns insemination event with said id otherwise null</returns>
        Task<InseminationEvent?> GetInseminationEventByIdAsync( int id );

        /// <summary>
        ///     Creates a new insemination event
        /// </summary>
        /// <param name="insemination" > Details of the insemination event</param>
        /// <returns>the created insemination event</returns>
        Task<int> CreateInseminationEventAsync( InseminationEvent insemination );

        /// <summary>
        ///     Updates an existing insemination event
        /// </summary>
        /// <param name="insemination" >Change details</param>
        Task UpdateInseminationEventAsync( InseminationEvent insemination );

        /// <summary>
        ///     Deletes insemination event from the database
        /// </summary>
        /// <param name="id" >Unique identification of insemination event</param>
        Task DeleteInseminationEventAsync( int id );
    }
}
