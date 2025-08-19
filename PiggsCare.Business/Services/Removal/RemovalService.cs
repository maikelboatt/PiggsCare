using PiggsCare.ApplicationState.Stores.Removal;
using PiggsCare.Business.Services.Errors;
using PiggsCare.DataAccess.Repositories.Removal;
using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.Removal
{
    /// <summary>
    ///     Service for managing removal events.
    /// </summary>
    /// <param name="removalEventRepository" >Repository for removal events.</param>
    public class RemovalService( IRemovalEventRepository removalEventRepository, IRemovalEventStore removalEventStore, IDatabaseErrorHandlerService databaseErrorHandlerService )
        :IRemovalService
    {
        /// <summary>
        ///     Retrieves all removal events for a specific animal.
        /// </summary>
        /// <param name="animalId" >The ID of the animal.</param>
        /// <returns>A collection of removal events.</returns>
        public async Task GetAllRemovalEventsAsync( int animalId )
        {
            IEnumerable<RemovalEvent> removals = await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => removalEventRepository.GetAllRemovalEventsAsync(animalId),
                "Retrieving all removals"
            ) ?? [];

            removalEventStore.LoadRemovalEvents(removals);
        }

        /// <summary>
        ///     Retrieves a removal event by its ID.
        /// </summary>
        /// <param name="removalEventId" >The ID of the removal event.</param>
        /// <returns>The removal event if found; otherwise, null.</returns>
        public RemovalEvent? GetRemovalEventByIdAsync( int removalEventId ) => removalEventStore.GetRemovalEventById(removalEventId);

        /// <summary>
        ///     Creates a new removal event.
        /// </summary>
        /// <param name="removalEvent" >The removal event to create.</param>
        public async Task CreateRemovalEventAsync( RemovalEvent removalEvent )
        {
            int? uniqueId = await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => removalEventRepository.CreateRemovalEventAsync(removalEvent),
                "Adding removal"
            );
            {
                RemovalEvent removalEventWithCorrectId = removalEventStore.CreateRemovalEventWithCorrectId(uniqueId.Value, removalEvent);
                removalEventStore.AddRemovalEvent(removalEventWithCorrectId);
            }
        }

        /// <summary>
        ///     Updates an existing removal event.
        /// </summary>
        /// <param name="removalEvent" >The removal event to update.</param>
        public async Task UpdateRemovalEventAsync( RemovalEvent removalEvent )
        {
            await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => removalEventRepository.UpdateRemovalEventAsync(removalEvent),
                "Updating removal"
            );
            removalEventStore.UpdateRemovalEvent(removalEvent);
        }

        /// <summary>
        ///     Deletes a removal event by its ID.
        /// </summary>
        /// <param name="removalEventId" >The ID of the removal event to delete.</param>
        public async Task DeleteRemovalEventAsync( int removalEventId )
        {
            await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => removalEventRepository.DeleteRemovalEventAsync(removalEventId),
                "Deleting removal"
            );
            removalEventStore.DeleteRemovalEvent(removalEventId);
        }
    }
}
