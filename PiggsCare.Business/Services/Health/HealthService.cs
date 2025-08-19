using PiggsCare.ApplicationState.Stores.Health;
using PiggsCare.Business.Services.Errors;
using PiggsCare.DataAccess.Repositories.Health;
using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.Health
{
    /// <summary>
    ///     Service for managing health records.
    /// </summary>
    /// <param name="recordRepository" >Repository for health records.</param>
    public class HealthService( IHealthRecordRepository recordRepository, IHealthRecordStore healthRecordStore, IDatabaseErrorHandlerService databaseErrorHandlerService ):IHealthService
    {
        /// <summary>
        ///     Retrieves all health records for a given ID.
        /// </summary>
        /// <param name="id" >The identifier for which to retrieve health records.</param>
        /// <returns>A collection of health records.</returns>
        public async Task GetAllHealthRecordsAsync( int id )
        {
            IEnumerable<HealthRecord>? scans = await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                                                   () => recordRepository.GetAllHealthRecordsAsync(id),
                                                   "Retrieve all health records")
                                               ?? [];
            healthRecordStore.LoadHealthRecords(scans);
        }

        /// <summary>
        ///     Retrieves a health record by its ID.
        /// </summary>
        /// <param name="id" >The health record identifier.</param>
        /// <returns>The health record if found; otherwise, null.</returns>
        public HealthRecord? GetHealthRecordByIdAsync( int id ) => healthRecordStore.GetHealthRecordById(id);

        /// <summary>
        ///     Creates a new health record.
        /// </summary>
        /// <param name="health" >The health record to create.</param>
        public async Task CreateHealthRecordAsync( HealthRecord health )
        {
            int? uniqueId = await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => recordRepository.CreateHealthRecordAsync(health),
                "Adding health record");
            {
                HealthRecord heathRecordWithCorrectId = healthRecordStore.CreateHealthRecordWithCorrectId(uniqueId.Value, health);
                healthRecordStore.AddHealthRecord(heathRecordWithCorrectId);
            }
        }

        /// <summary>
        ///     Updates an existing health record.
        /// </summary>
        /// <param name="health" >The health record to update.</param>
        public async Task UpdateHealthRecordAsync( HealthRecord health )
        {
            await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => recordRepository.UpdateHealthRecordAsync(health),
                "Updating health record");

            healthRecordStore.UpdateHealthRecord(health);
        }

        /// <summary>
        ///     Deletes a health record by its ID.
        /// </summary>
        /// <param name="healthId" >The identifier of the health record to delete.</param>
        public async Task DeleteHealthRecordAsync( int healthId )
        {
            await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => recordRepository.DeleteHealthRecordAsync(healthId),
                "Deleting health record");

            healthRecordStore.DeleteHealthRecord(healthId);
        }
    }
}
