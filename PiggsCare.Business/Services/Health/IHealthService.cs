using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.Health
{
    /// <summary>
    ///     Service interface for managing health records.
    /// </summary>
    public interface IHealthService
    {
        /// <summary>
        ///     Retrieves all health records for a given entity by its identifier.
        /// </summary>
        /// <param name="id" >The identifier of the entity.</param>
        /// <returns>A collection of health records.</returns>
        Task GetAllHealthRecordsAsync( int id );

        /// <summary>
        ///     Retrieves a specific health record by its identifier.
        /// </summary>
        /// <param name="id" >The identifier of the health record.</param>
        /// <returns>The health record if found; otherwise, null.</returns>
        HealthRecord? GetHealthRecordByIdAsync( int id );

        /// <summary>
        ///     Creates a new health record.
        /// </summary>
        /// <param name="health" >The health record to create.</param>
        Task CreateHealthRecordAsync( HealthRecord health );

        /// <summary>
        ///     Updates an existing health record.
        /// </summary>
        /// <param name="health" >The health record to update.</param>
        Task UpdateHealthRecordAsync( HealthRecord health );

        /// <summary>
        ///     Deletes a health record by its identifier.
        /// </summary>
        /// <param name="healthId" >The identifier of the health record to delete.</param>
        Task DeleteHealthRecordAsync( int healthId );
    }
}
