using Microsoft.Extensions.Logging;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Helpers;

namespace PiggsCare.ApplicationState.Stores.ScheduledNotifications
{
    public class ScheduledNotificationStore( ILogger<ScheduledNotificationStore> logger ):IScheduledNotificationStore
    {
        private readonly ReaderWriterLockSlim _lock = new();
        private readonly ILogger<ScheduledNotificationStore> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly List<ScheduledNotification> _notifications = [];

        public IEnumerable<ScheduledNotification> ScheduledNotifications
        {
            get
            {
                using (_lock.Read())
                {
                    return _notifications.ToList();
                }
            }
        }

        public void LoadScheduledNotifications( IEnumerable<ScheduledNotification> notifications )
        {
            try
            {
                using (_lock.Write())
                {
                    _notifications.Clear();
                    _notifications.AddRange(notifications);
                }

                OnScheduledNotificationsLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error loading scheduled notifications: {ExMessage}", ex.Message);
                throw;
            }
        }

        public ScheduledNotification? GetScheduledNotificationById( int id )
        {
            using (_lock.Read())
            {
                return _notifications.FirstOrDefault(n => n.Id == id);
            }
        }

        public void AddScheduledNotification( ScheduledNotification notification )
        {
            using (_lock.Write())
            {
                _notifications.Add(notification);
                OnScheduledNotificationAdded?.Invoke(notification);
            }
        }

        public ScheduledNotification CreateScheduledNotificationWithCorrectId( int id, ScheduledNotification notification ) => new(
            id,
            notification.Message,
            notification.ScheduledDate,
            notification.AnimalIds,
            notification.SynchronizationId
        );

        public void UpdateScheduledNotification( ScheduledNotification notification )
        {
            using (_lock.Write())
            {
                int index = _notifications.FindIndex(n => n.Id == notification.Id);
                if (index == -1) return;
                _notifications[index] = notification;
                OnScheduledNotificationUpdated?.Invoke(notification);
            }
        }

        public void DeleteScheduledNotification( int id )
        {
            using (_lock.Write())
            {
                ScheduledNotification? notification = GetScheduledNotificationById(id);
                if (notification == null) return;

                _notifications.Remove(notification);
                OnScheduledNotificationDeleted?.Invoke(notification);
            }
        }

        public event Action? OnScheduledNotificationsLoaded;
        public event Action<ScheduledNotification>? OnScheduledNotificationAdded;
        public event Action<ScheduledNotification>? OnScheduledNotificationUpdated;
        public event Action<ScheduledNotification>? OnScheduledNotificationDeleted;
    }
}
