using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.ScheduledNotifications
{
    /// <summary>
    ///     Defines the contract for managing scheduled notifications, including retrieval,
    ///     creation, updating, and deletion operations.
    /// </summary>
    public interface IScheduledNotificationService
    {
        /// <summary>
        ///     Retrieves all scheduled notifications for a specific date asynchronously and updates the notification store.
        /// </summary>
        /// <param name="scheduledDate" >The exact date for which to retrieve scheduled notifications.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task GetScheduledNotificationsByExactDateAsync( DateOnly scheduledDate );

        /// <summary>
        ///     Retrieves a scheduled notification by its unique identifier.
        /// </summary>
        /// <param name="id" >The unique identifier of the scheduled notification.</param>
        /// <returns>The scheduled notification if found; otherwise, null.</returns>
        ScheduledNotification? GetScheduledNotificationById( int id );

        /// <summary>
        ///     Creates a new scheduled notification asynchronously.
        /// </summary>
        /// <param name="notification" >The scheduled notification to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateScheduledNotificationAsync( ScheduledNotification notification );

        /// <summary>
        ///     Updates an existing scheduled notification asynchronously.
        /// </summary>
        /// <param name="notification" >The scheduled notification to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateScheduledNotificationAsync( ScheduledNotification notification );

        /// <summary>
        ///     Deletes a scheduled notification by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id" >The unique identifier of the scheduled notification to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteScheduledNotificationAsync( int id );

        // List<Animal> GetAnimalsByIds( List<int> animalIds );
    }
}
