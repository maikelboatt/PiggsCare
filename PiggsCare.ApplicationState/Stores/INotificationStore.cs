using PiggsCare.Domain.Models;

namespace PiggsCare.ApplicationState.Stores
{
    /// <summary>
    ///     Defines a store for managing notifications and scheduled notifications.
    /// </summary>
    public interface INotificationStore
    {
        /// <summary>
        ///     Gets or sets the list of immediate notifications.
        /// </summary>
        List<string> Notifications { get; set; }

        /// <summary>
        ///     Gets or sets the list of scheduled notifications.
        /// </summary>
        List<ScheduledNotification> ScheduledNotifications { get; set; }

        /// <summary>
        ///     Adds a new notification to the store.
        /// </summary>
        /// <param name="notification" >The notification message to add.</param>
        void AddNotification( string notification );

        /// <summary>
        ///     Adds a new scheduled notification for the specified animals.
        /// </summary>
        /// <param name="animals" >The list of animals associated with the notification.</param>
        /// <param name="message" >The notification message.</param>
        /// <param name="scheduledDate" >The date and time when the notification should be triggered.</param>
        void AddScheduledNotification( List<int> animals, string message, DateTime scheduledDate, int id );

        /// <summary>
        ///     Occurs when the notifications list changes.
        /// </summary>
        event Action NotificationsChanged;

        /// <summary>
        ///     Occurs when the scheduled notifications list changes.
        /// </summary>
        event Action ScheduledNotificationsChanged;
    }
}
