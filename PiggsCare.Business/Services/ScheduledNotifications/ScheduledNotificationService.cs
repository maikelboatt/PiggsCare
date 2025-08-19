using PiggsCare.ApplicationState.Stores.ScheduledNotifications;
using PiggsCare.Business.Services.Errors;
using PiggsCare.DataAccess.Repositories.ScheduledNotifications;
using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.ScheduledNotifications
{
    /// <summary>
    ///     Service for managing scheduled notification-related operations.
    /// </summary>
    /// <param name="notificationRepository" >
    ///     Repository for scheduled notification data access.
    /// </param>
    /// <param name="notificationStore" >
    ///     Store for scheduled notification state management.
    /// </param>
    /// <param name="databaseErrorHandlerService" >
    ///     Service for handling database operation errors.
    /// </param>
    public class ScheduledNotificationService(
        IScheduledNotificationRepository notificationRepository,
        IScheduledNotificationStore notificationStore,
        IDatabaseErrorHandlerService databaseErrorHandlerService ):IScheduledNotificationService
    {
        /// <summary>
        ///     Retrieves all scheduled notifications for a specific date asynchronously and updates the notification store.
        /// </summary>
        /// <param name="scheduledDate" >The exact date for which to retrieve scheduled notifications.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GetScheduledNotificationsByExactDateAsync( DateOnly scheduledDate )
        {
            IEnumerable<ScheduledNotification> scheduledNotifications = await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                                                                            () => notificationRepository.GetScheduledNotificationsByExactDateAsync(scheduledDate),
                                                                            "Retrieving all scheduled notifications by date")
                                                                        ?? [];
            notificationStore.LoadScheduledNotifications(scheduledNotifications);
        }

        /// <summary>
        ///     Retrieves a scheduled notification by its unique identifier from the store.
        /// </summary>
        /// <param name="id" >The unique identifier of the scheduled notification.</param>
        /// <returns>The scheduled notification if found; otherwise, null.</returns>
        public ScheduledNotification? GetScheduledNotificationById( int id ) => notificationStore.GetScheduledNotificationById(id);

        /// <summary>
        ///     Creates a new scheduled notification asynchronously and adds it to the store.
        /// </summary>
        /// <param name="notification" >The scheduled notification to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateScheduledNotificationAsync( ScheduledNotification notification )
        {
            int? uniqueId = await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => notificationRepository.CreateScheduledNotificationAsync(notification),
                "Adding scheduled notification"
            );
            {
                ScheduledNotification notificationWithId = notificationStore.CreateScheduledNotificationWithCorrectId(uniqueId.Value, notification);
                notificationStore.AddScheduledNotification(notificationWithId);
            }
        }

        /// <summary>
        ///     Updates an existing scheduled notification asynchronously and updates it in the store.
        /// </summary>
        /// <param name="notification" >The scheduled notification to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateScheduledNotificationAsync( ScheduledNotification notification )
        {
            await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => notificationRepository.UpdateScheduledNotificationAsync(notification),
                "Updating scheduled notification"
            );
            notificationStore.UpdateScheduledNotification(notification);
        }

        /// <summary>
        ///     Deletes a scheduled notification by its unique identifier asynchronously and removes it from the store.
        /// </summary>
        /// <param name="id" >The unique identifier of the scheduled notification to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteScheduledNotificationAsync( int id )
        {
            await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => notificationRepository.DeleteScheduledNotificationAsync(id),
                "Deleting scheduled notification"
            );
            notificationStore.DeleteScheduledNotification(id);
        }

        // public List<Animal> GetAnimalsByIds( List<int> animalIds ) => animalIds.Select(animalService.GetAnimalById).OfType<Animal>().ToList();
    }
}
