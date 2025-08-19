using PiggsCare.Domain.Models;

namespace PiggsCare.DataAccess.Repositories.Farrowing
{
    public interface IFarrowRepository
    {
        /// <summary>
        ///     Method to Get all FarrowEvent in the database
        /// </summary>
        /// <param name="id" ></param>
        /// <returns>All farrow event in the database</returns>
        Task<IEnumerable<FarrowEvent>> GetAllFarrowEventAsync( int id );

        /// <summary>
        ///     Method that returns  farrow event that has a unique id
        /// </summary>
        /// <param name="id" >Unique identification of farrow event </param>
        /// <returns>Returns farrow event with said id otherwise null</returns>
        Task<FarrowEvent?> GetFarrowEventByIdAsync( int id );

        /// <summary>
        ///     Creates a new farrow event
        /// </summary>
        /// <param name="farrow" > Details of the farrow event</param>
        /// <returns>the created farrow event</returns>
        Task<int> CreateFarrowEventAsync( FarrowEvent farrow );

        /// <summary>
        ///     Updates an existing farrow event
        /// </summary>
        /// <param name="farrow" >Change details</param>
        Task UpdateFarrowEventAsync( FarrowEvent farrow );

        /// <summary>
        ///     Deletes  farrow event from the database
        /// </summary>
        /// <param name="id" >Unique identification of farrow event</param>
        Task DeleteFarrowEventAsync( int id );
    }
}
