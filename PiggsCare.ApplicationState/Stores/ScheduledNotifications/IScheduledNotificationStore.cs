using PiggsCare.Domain.Models;

namespace PiggsCare.ApplicationState.Stores.ScheduledNotifications
{
    public interface IScheduledNotificationStore
    {
        IEnumerable<ScheduledNotification> ScheduledNotifications { get; }

        void LoadScheduledNotifications( IEnumerable<ScheduledNotification> notifications );

        ScheduledNotification? GetScheduledNotificationById( int id );

        void AddScheduledNotification( ScheduledNotification notification );

        ScheduledNotification CreateScheduledNotificationWithCorrectId( int id, ScheduledNotification notification );

        void UpdateScheduledNotification( ScheduledNotification notification );

        void DeleteScheduledNotification( int id );

        event Action? OnScheduledNotificationsLoaded;
        event Action<ScheduledNotification>? OnScheduledNotificationAdded;
        event Action<ScheduledNotification>? OnScheduledNotificationUpdated;
        event Action<ScheduledNotification>? OnScheduledNotificationDeleted;
    }
}
