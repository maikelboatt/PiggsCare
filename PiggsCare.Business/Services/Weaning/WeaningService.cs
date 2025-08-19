using PiggsCare.ApplicationState.Stores.Weaning;
using PiggsCare.Business.Services.Errors;
using PiggsCare.DataAccess.Repositories.Weaning;
using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.Weaning
{
    /// <summary>
    ///     Service for managing weaning events.
    /// </summary>
    public class WeaningService( IWeaningRepository weaningRepository, IWeaningStore weaningStore, IDatabaseErrorHandlerService databaseErrorHandlerService ):IWeaningService
    {
        /// <summary>
        ///     Retrieves all weaning events for a given identifier.
        /// </summary>
        /// <param name="id" >The identifier to filter weaning events.</param>
        /// <returns>A collection of <see cref="WeaningEvent" />.</returns>
        public async Task GetAllWeaningEventAsync( int id )
        {
            IEnumerable<WeaningEvent> weaning = await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => weaningRepository.GetAllWeaningEventAsync(id),
                "Retrieving all weaning"
            ) ?? [];
            weaningStore.LoadWeaningEvents(weaning);
        }

        /// <summary>
        ///     Retrieves a specific weaning event by its identifier.
        /// </summary>
        /// <param name="id" >The identifier of the weaning event.</param>
        /// <returns>The <see cref="WeaningEvent" /> if found; otherwise, null.</returns>
        public WeaningEvent? GetWeaningEventByIdAsync( int id ) => weaningStore.GetWeaningEventById(id);

        /// <summary>
        ///     Creates a new weaning event.
        /// </summary>
        /// <param name="weaning" >The weaning event to create.</param>
        public async Task CreateWeaningEventAsync( WeaningEvent weaning )
        {
            int? uniqueId = await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => weaningRepository.CreateWeaningEventAsync(weaning),
                "Adding weaning"
            );
            {
                WeaningEvent weaningWithCorrectId = weaningStore.CreateWeaningEventWithCorrectId(uniqueId.Value, weaning);
                weaningStore.AddWeaningEvent(weaningWithCorrectId);
            }
        }

        /// <summary>
        ///     Updates an existing weaning event.
        /// </summary>
        /// <param name="weaning" >The weaning event to update.</param>
        public async Task UpdateWeaningEventAsync( WeaningEvent weaning )
        {
            await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => weaningRepository.UpdateWeaningEventAsync(weaning),
                "Updating weaning"
            );
            weaningStore.UpdateWeaningEvent(weaning);
        }

        /// <summary>
        ///     Deletes a weaning event by its identifier.
        /// </summary>
        /// <param name="weaningId" >The identifier of the weaning event to delete.</param>
        public async Task DeleteWeaningEventAsync( int weaningId )
        {
            await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => weaningRepository.DeleteWeaningEventAsync(weaningId),
                "Deleting weaning"
            );
            weaningStore.DeleteWeaningEvent(weaningId);
        }
    }
}
