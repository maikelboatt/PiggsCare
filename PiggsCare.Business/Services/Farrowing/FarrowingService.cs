using PiggsCare.ApplicationState.Stores.Farrowing;
using PiggsCare.Business.Services.Errors;
using PiggsCare.DataAccess.Repositories.Farrowing;
using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.Farrowing
{
    public class FarrowingService( IFarrowRepository farrowingRepository, IFarrowingStore farrowingStore, IDatabaseErrorHandlerService databaseErrorHandlerService ):IFarrowingService
    {
        public async Task GetAllFarrowingsAsync( int id )
        {
            IEnumerable<FarrowEvent> farrowings = await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => farrowingRepository.GetAllFarrowEventAsync(id),
                "Retrieving all farrowings"
            ) ?? [];
            farrowingStore.LoadFarrowingEvent(farrowings);
        }

        public FarrowEvent? GetFarrowingById( int id ) => farrowingStore.GetFarrowingById(id);


        public async Task CreateFarrowingAsync( FarrowEvent farrowing )
        {
            int? uniqueId = await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => farrowingRepository.CreateFarrowEventAsync(farrowing),
                "Adding farrowing"
            );
            {
                FarrowEvent farrowingWithCorrectId = farrowingStore.CreateFarrowingWithCorrectId(uniqueId.Value, farrowing);
                farrowingStore.AddFarrowing(farrowingWithCorrectId);
            }
        }

        public async Task UpdateFarrowingAsync( FarrowEvent farrowing )
        {
            await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => farrowingRepository.UpdateFarrowEventAsync(farrowing),
                "Updating farrowing"
            );
            farrowingStore.UpdateFarrowing(farrowing);
        }

        public async Task DeleteFarrowingAsync( int farrowingId )
        {
            await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => farrowingRepository.DeleteFarrowEventAsync(farrowingId),
                "Deleting farrowing"
            );
            farrowingStore.DeleteFarrowing(farrowingId);
        }
    }
}
