using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.Pregnancy
{
    public interface IPregnancyService
    {
        /// <summary>
        ///     Retrieves all pregnancy scans for a given ID.
        /// </summary>
        /// <param name="id" >The identifier for which to retrieve scans.</param>
        /// <returns>A collection of <see cref="PregnancyScan" /> objects.</returns>
        Task GetAllPregnancyScansAsync( int id );

        /// <summary>
        ///     Retrieves a pregnancy scan by its ID.
        /// </summary>
        /// <param name="id" >The scan identifier.</param>
        /// <returns>The <see cref="PregnancyScan" /> if found; otherwise, null.</returns>
        PregnancyScan? GetPregnancyScanByIdAsync( int id );

        /// <summary>
        ///     Creates a new pregnancy scan record.
        /// </summary>
        /// <param name="scan" >The <see cref="PregnancyScan" /> to create.</param>
        Task CreatePregnancyScanAsync( PregnancyScan scan );

        /// <summary>
        ///     Updates an existing pregnancy scan record.
        /// </summary>
        /// <param name="scan" >The <see cref="PregnancyScan" /> to update.</param>
        Task UpdatePregnancyScanAsync( PregnancyScan scan );

        /// <summary>
        ///     Deletes a pregnancy scan record by its ID.
        /// </summary>
        /// <param name="scanId" >The identifier of the scan to delete.</param>
        Task DeletePregnancyScanAsync( int scanId );
    }
}
