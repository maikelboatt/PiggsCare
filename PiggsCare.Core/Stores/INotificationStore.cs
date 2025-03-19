namespace PiggsCare.Core.Stores
{
    public interface INotificationStore
    {
        List<string> Notifications { get; set; }

        void AddNotification( string notification );

        event Action NotificationsChanged;
    }
}
