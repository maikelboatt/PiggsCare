using Microsoft.Extensions.Logging;
using PiggsCare.ApplicationState.Stores.Animals;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Helpers;

namespace PiggsCare.ApplicationState.Stores.ScheduledNotifications
{
    public class ScheduleNotificationAnimalStore:IScheduleNotificationAnimalStore
    {
        private readonly List<Animal> _animalsList = [];
        private readonly IAnimalStore _animalStore;

        private readonly ReaderWriterLockSlim _lock = new();
        private readonly ILogger<ScheduleNotificationAnimalStore> _logger;

        public ScheduleNotificationAnimalStore( IAnimalStore animalStore, ILogger<ScheduleNotificationAnimalStore> logger )
        {
            _animalStore = animalStore ?? throw new ArgumentNullException(nameof(animalStore));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public IEnumerable<Animal> Animals
        {
            get
            {
                using (_lock.Read())
                {
                    return _animalsList.ToList();
                }
            }
        }

        public void LoadAnimals( IEnumerable<int> animalIds )
        {
            try
            {
                using (_lock.Write())
                {
                    _animalsList.Clear();

                    HashSet<int> idSet = new(animalIds);
                    IEnumerable<Animal> matchedAnimals = _animalStore.Animals
                                                                     .Where(animal => idSet.Contains(animal.AnimalId));

                    foreach (Animal animal in matchedAnimals)
                    {
                        _animalsList.Add(animal);
                    }
                }

                OnScheduledNotificationsAnimalLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error seeding animal list: {ExMessage}", ex.Message);
                throw;
            }
        }

        public event Action? OnScheduledNotificationsAnimalLoaded;
        public event Action<ScheduledNotification>? OnScheduledNotificationAnimalDeleted;
    }
}
