using PiggsCare.Domain.Models;

namespace PiggsCare.Domain.Services
{
    public interface IWeaningService
    {
        /// <summary>
        ///     Method to Get all WeaningEvent in the database
        /// </summary>
        /// <param name="id" ></param>
        /// <returns>All weaning event in the database</returns>
        Task<IEnumerable<WeaningEvent>> GetAllWeaningEventAsync( int id );

        /// <summary>
        ///     Method that returns  weaning event that has a unique id
        /// </summary>
        /// <param name="id" >Unique identification of weaning event </param>
        /// <returns>Returns weaning event with said id otherwise null</returns>
        Task<WeaningEvent?> GetWeaningEventByIdAsync( int id );

        /// <summary>
        ///     Creates a new weaning event
        /// </summary>
        /// <param name="weaning" > Details of the weaning event</param>
        /// <returns>the created weaning event</returns>
        Task CreateWeaningEventAsync( WeaningEvent weaning );

        /// <summary>
        ///     Updates an existing weaning event
        /// </summary>
        /// <param name="weaning" >Change details</param>
        Task UpdateWeaningEventAsync( WeaningEvent weaning );

        /// <summary>
        ///     Deletes  weaning event from the database
        /// </summary>
        /// <param name="id" >Unique identification of weaning event</param>
        Task DeleteWeaningEventAsync( int id );
    }
}
