using PiggsCare.ApplicationState.Stores.Insemination;
using PiggsCare.Business.Services.Errors;
using PiggsCare.DataAccess.Repositories.Insemination;
using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.Insemination
{
    /// <summary>
    ///     Service for managing insemination events.
    /// </summary>
    /// <param name="inseminationEventRepository" >Repository for insemination events.</param>
    public class InseminationService(
        IInseminationEventRepository inseminationEventRepository,
        IInseminationEventStore inseminationEventStore,
        IDatabaseErrorHandlerService databaseErrorHandlerService ):IInseminationService
    {
        /// <summary>
        ///     Retrieves all insemination events for a given id.
        /// </summary>
        /// <param name="id" >The identifier for which to retrieve insemination events.</param>
        /// <returns>A collection of <see cref="InseminationEvent" />.</returns>
        public async Task GetAllInseminationEventsAsync( int id )
        {
            IEnumerable<InseminationEvent> inseminations = await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => inseminationEventRepository.GetAllInseminationEventsAsync(id),
                "Retrieving all inseminations"
            ) ?? [];
            inseminationEventStore.LoadInseminationEvents(inseminations);
        }

        /// <summary>
        ///     Retrieves a specific insemination event by its id.
        /// </summary>
        /// <param name="id" >The insemination event identifier.</param>
        /// <returns>The <see cref="InseminationEvent" /> if found; otherwise, null.</returns>
        public InseminationEvent? GetInseminationEventByIdAsync( int id ) => inseminationEventStore.GetInseminationEventById(id);

        public InseminationEvent? GetInseminationEventBySynchronizationIdAsync( int synchronizationId ) =>
            inseminationEventStore.GetInseminationEventBySynchronizationId(synchronizationId);

        /// <summary>
        ///     Creates a new insemination event.
        /// </summary>
        /// <param name="insemination" >The insemination event to create.</param>
        public async Task CreateInseminationEventAsync( InseminationEvent insemination )
        {
            int? uniqueId = await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => inseminationEventRepository.CreateInseminationEventAsync(insemination),
                "Adding Insemination"
            );
            {
                InseminationEvent inseminationWithCorrectId = inseminationEventStore.CreateInseminationEventWithCorrectId(uniqueId.Value, insemination);
                inseminationEventStore.AddInseminationEvent(inseminationWithCorrectId);
            }
        }

        /// <summary>
        ///     Updates an existing insemination event.
        /// </summary>
        /// <param name="insemination" >The insemination event to update.</param>
        public async Task UpdateInseminationEventAsync( InseminationEvent insemination )
        {
            await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => inseminationEventRepository.UpdateInseminationEventAsync(insemination),
                "Updating insemination"
            );
            inseminationEventStore.UpdateInseminationEvent(insemination);
        }

        /// <summary>
        ///     Deletes an insemination event by its inseminationId.
        /// </summary>
        /// <param name="inseminationId" >The identifier of the insemination event to delete.</param>
        public async Task DeleteInseminationEventAsync( int inseminationId )
        {
            await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => inseminationEventRepository.DeleteInseminationEventAsync(inseminationId),
                "Deleting insemination"
            );
            inseminationEventStore.DeleteInseminationEvent(inseminationId);
        }

        /// <summary>
        ///     Retrieves all insemination events with animal details by synchronization batch.
        /// </summary>
        /// <param name="synchronizationId" >The synchronization batch identifier.</param>
        /// <returns>A collection of <see cref="InseminationEventWithAnimal" />.</returns>
        public void GetAllInseminationEventBySynchronizationBatchAsync( int synchronizationId ) =>
            inseminationEventStore.GetAllInseminationEventsBySynchronizationBatch(synchronizationId);
    }
}
