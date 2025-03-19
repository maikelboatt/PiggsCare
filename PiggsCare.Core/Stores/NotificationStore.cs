namespace PiggsCare.Core.Stores
{
    public class NotificationStore:INotificationStore
    {
        public List<string> Notifications { get; set; } = [];
       

        public void AddNotification( string notification )
        {
            Notifications.Add(notification);
            NotificationsChanged?.Invoke();
        }

        public event Action? NotificationsChanged;
    }
}
