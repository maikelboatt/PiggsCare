using PiggsCare.Domain.Models;

namespace PiggsCare.Domain.Repositories
{
    public interface IHealthRecordRepository
    {
        /// <summary>
        ///     Method to Get all HealthRecords in the database
        /// </summary>
        /// <param name="id" ></param>
        /// <returns>All health records in the database</returns>
        Task<IEnumerable<HealthRecord>> GetAllHealthRecordsAsync( int id );

        /// <summary>
        ///     Method that returns  health-record that has a unique id
        /// </summary>
        /// <param name="id" >Unique identification of health-record </param>
        /// <returns>Returns health-record with said id otherwise null</returns>
        Task<HealthRecord?> GetHealthRecordByIdAsync( int id );

        /// <summary>
        ///     Creates a new health record
        /// </summary>
        /// <param name="health" > Details of the health record</param>
        /// <returns>the created health record</returns>
        Task CreateHealthRecordAsync( HealthRecord health );

        /// <summary>
        ///     Updates an existing record
        /// </summary>
        /// <param name="health" >Change details</param>
        Task UpdateHealthRecordAsync( HealthRecord health );

        /// <summary>
        ///     Deletes  health record from the database
        /// </summary>
        /// <param name="id" >Unique identification of health record</param>
        Task DeleteHealthRecordAsync( int id );
    }
}
