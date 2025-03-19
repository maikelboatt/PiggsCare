using PiggsCare.Domain.Models;

namespace PiggsCare.Domain.Services
{
    public interface IPregnancyService
    {
        /// <summary>
        ///     Method to Get all Pregnancy Scan in the database
        /// </summary>
        /// <param name="id" ></param>
        /// <returns>All pregnancy scan in the database</returns>
        Task<IEnumerable<PregnancyScan>> GetAllPregnancyScansAsync( int id );

        /// <summary>
        ///     Method that returns pregnancy scan that has a unique id
        /// </summary>
        /// <param name="id" >Unique identification of pregnancy scan </param>
        /// <returns>Returns pregnancy scan with said id otherwise null</returns>
        Task<PregnancyScan?> GetPregnancyScanByIdAsync( int id );

        /// <summary>
        ///     Creates a new pregnancy scan
        /// </summary>
        /// <param name="scan" > Details of the pregnancy scan</param>
        /// <returns>the created pregnancy scan </returns>
        Task CreatePregnancyScanAsync( PregnancyScan scan );

        /// <summary>
        ///     Updates an existing pregnancy scan
        /// </summary>
        /// <param name="scan" >Change details</param>
        Task UpdatePregnancyScanAsync( PregnancyScan scan );

        /// <summary>
        ///     Deletes pregnancy scan record from the database
        /// </summary>
        /// <param name="id" >Unique identification of pregnancy scan</param>
        Task DeleteHealthRecordAsync( int id );
    }
}
