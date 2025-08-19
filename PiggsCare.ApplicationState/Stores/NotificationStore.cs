using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Services;

namespace PiggsCare.ApplicationState.Stores
{
    public class NotificationStore( IDateConverterService dateConverterService ):INotificationStore
    {
        public List<string> Notifications { get; set; } = [];
        public List<ScheduledNotification> ScheduledNotifications { get; set; } = [];


        public void AddNotification( string notification )
        {
            Notifications.Add(notification);
            NotificationsChanged?.Invoke();
        }

        public event Action? NotificationsChanged;
        public event Action? ScheduledNotificationsChanged;

        public void AddScheduledNotification( List<int> animals, string message, DateTime scheduledDate, int id )
        {
            ScheduledNotifications.Add(
                new ScheduledNotification(
                    0, // Assuming ID will be set by the database
                    message,
                    dateConverterService.GetDateOnly(scheduledDate),
                    animals.ToList(),
                    id
                ));
        }
    }
}
