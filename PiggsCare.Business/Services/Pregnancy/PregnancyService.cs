using PiggsCare.ApplicationState.Stores.Pregnancy;
using PiggsCare.Business.Services.Errors;
using PiggsCare.DataAccess.Repositories.Pregnancy;
using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.Pregnancy
{
    /// <summary>
    ///     Service for managing pregnancy scans.
    /// </summary>
    public class PregnancyService( IPregnancyRepository pregnancyRepository, IPregnancyStore pregnancyStore, IDatabaseErrorHandlerService databaseErrorHandlerService ):IPregnancyService
    {
        /// <summary>
        ///     Retrieves all pregnancy scans for a given ID.
        /// </summary>
        /// <param name="id" >The identifier for which to retrieve scans.</param>
        /// <returns>A collection of <see cref="PregnancyScan" /> objects.</returns>
        public async Task GetAllPregnancyScansAsync( int id )
        {
            IEnumerable<PregnancyScan> farrowings = await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => pregnancyRepository.GetAllPregnancyScansAsync(id),
                "Retrieving all pregnancies"
            ) ?? [];
            pregnancyStore.LoadPregnancy(farrowings);
        }

        /// <summary>
        ///     Retrieves a pregnancy scan by its ID.
        /// </summary>
        /// <param name="id" >The scan identifier.</param>
        /// <returns>The <see cref="PregnancyScan" /> if found; otherwise, null.</returns>
        public PregnancyScan? GetPregnancyScanByIdAsync( int id ) => pregnancyStore.GetPregnancyById(id);

        /// <summary>
        ///     Creates a new pregnancy scan record.
        /// </summary>
        /// <param name="scan" >The <see cref="PregnancyScan" /> to create.</param>
        public async Task CreatePregnancyScanAsync( PregnancyScan scan )
        {
            int? uniqueId = await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => pregnancyRepository.CreatePregnancyScanAsync(scan),
                "Adding pregnancy scan"
            );
            {
                PregnancyScan pregnancyScanWithCorrectId = pregnancyStore.CreatePregnancyScanWithCorrectId(uniqueId.Value, scan);
                pregnancyStore.AddPregnancy(pregnancyScanWithCorrectId);
            }
        }

        /// <summary>
        ///     Updates an existing pregnancy scan record.
        /// </summary>
        /// <param name="scan" >The <see cref="PregnancyScan" /> to update.</param>
        public async Task UpdatePregnancyScanAsync( PregnancyScan scan )
        {
            await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => pregnancyRepository.UpdatePregnancyScanAsync(scan),
                "Updating pregnancy scan"
            );
            pregnancyStore.UpdatePregnancy(scan);
        }

        /// <summary>
        ///     Deletes a pregnancy scan record by its ID.
        /// </summary>
        /// <param name="scanId" >The identifier of the scan to delete.</param>
        public async Task DeletePregnancyScanAsync( int scanId )
        {
            await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => pregnancyRepository.DeletePregnancyScanAsync(scanId),
                "Deleting pregnancy scan"
            );
            pregnancyStore.DeletePregnancy(scanId);
        }
    }
}
