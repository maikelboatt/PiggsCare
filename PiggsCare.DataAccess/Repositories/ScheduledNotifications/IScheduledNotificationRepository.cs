using PiggsCare.Domain.Models;

namespace PiggsCare.DataAccess.Repositories.ScheduledNotifications
{
    /// <summary>
    ///     Repository interface for managing scheduled notifications.
    /// </summary>
    public interface IScheduledNotificationRepository
    {
        /// <summary>
        ///     Creates a new scheduled notification.
        /// </summary>
        /// <param name="notification" >The scheduled notification to create.</param>
        /// <returns>The ID of the created notification.</returns>
        Task<int> CreateScheduledNotificationAsync( ScheduledNotification notification );

        /// <summary>
        ///     Retrieves scheduled notifications for the exact specified date.
        /// </summary>
        /// <param name="scheduledDate" >The date to filter notifications by.</param>
        /// <returns>A collection of scheduled notifications for the given date.</returns>
        Task<IEnumerable<ScheduledNotification>> GetScheduledNotificationsByExactDateAsync( DateOnly scheduledDate );

        /// <summary>
        ///     Updates an existing scheduled notification.
        /// </summary>
        /// <param name="notification" >The scheduled notification to update.</param>
        Task UpdateScheduledNotificationAsync( ScheduledNotification notification );

        /// <summary>
        ///     Deletes a scheduled notification by its ID.
        /// </summary>
        /// <param name="id" >The ID of the notification to delete.</param>
        Task DeleteScheduledNotificationAsync( int id );
    }
}
