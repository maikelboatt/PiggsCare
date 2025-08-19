using PiggsCare.Domain.Models;

namespace PiggsCare.ApplicationState.Stores.ScheduledNotifications
{
    public interface IScheduleNotificationAnimalStore
    {
        IEnumerable<Animal> Animals { get; }

        void LoadAnimals( IEnumerable<int> animalIds );

        event Action? OnScheduledNotificationsAnimalLoaded;
        event Action<ScheduledNotification>? OnScheduledNotificationAnimalDeleted;
    }
}
