using Microsoft.Extensions.Logging;
using PiggsCare.ApplicationState.Stores.Animals;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Helpers;

namespace PiggsCare.ApplicationState.Stores.Insemination
{
    public class InseminationEventStore( ILogger<InseminationEventStore> logger, IAnimalStore animalStore ):IInseminationEventStore
    {
        private readonly List<InseminationEvent> _inseminationEvents = [];
        private readonly List<InseminationEventWithAnimal> _inseminationEventsWithAnimals = [];
        private readonly ReaderWriterLockSlim _lock = new();
        private readonly ILogger<InseminationEventStore> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public IEnumerable<InseminationEventWithAnimal> InseminationEventsWithAnimals
        {
            get
            {
                using (_lock.Read())
                {
                    return _inseminationEventsWithAnimals.ToList();
                }
            }
        }

        public IEnumerable<InseminationEvent> InseminationEvents
        {
            get
            {
                using (_lock.Read())
                {
                    return _inseminationEvents.ToList();
                }
            }
        }


        public void LoadInseminationEvents( IEnumerable<InseminationEvent> events, CancellationToken cancellationToken = default )
        {
            try
            {
                using (_lock.Write())
                {
                    _inseminationEvents.Clear();
                    _inseminationEvents.AddRange(events);
                }

                OnInseminationEventsLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error loading insemination events: {ExMessage}", ex.Message);
                throw;
            }
        }

        public InseminationEvent? GetInseminationEventById( int id )
        {
            using (_lock.Read())
            {
                return _inseminationEvents.FirstOrDefault(e => e.BreedingEventId == id);
            }
        }

        public InseminationEvent? GetInseminationEventBySynchronizationId( int synchronizationId )
        {
            using (_lock.Read())
            {
                return _inseminationEvents.FirstOrDefault(e => e.SynchronizationEventId == synchronizationId);

            }
        }

        public void GetAllInseminationEventsBySynchronizationBatch( int synchronizationId )
        {
            using (_lock.Read())
            {
                List<InseminationEventWithAnimal> results = _inseminationEvents
                                                            .Where(e => e.SynchronizationEventId == synchronizationId)
                                                            .Select(e => new InseminationEventWithAnimal(
                                                                        e.BreedingEventId,
                                                                        e.AnimalId,
                                                                        e.AiDate,
                                                                        e.ExpectedFarrowDate,
                                                                        e.SynchronizationEventId,
                                                                        GetAnimalNameForInsemination(e.AnimalId) // Assuming name is not available in this context
                                                                    ))
                                                            .ToList();
                _inseminationEventsWithAnimals.Clear();
                _inseminationEventsWithAnimals.AddRange(results);
            }
        }

        public InseminationEvent CreateInseminationEventWithCorrectId( int inseminationEventId, InseminationEvent inseminationEvent ) => new(
            inseminationEventId,
            inseminationEvent.AnimalId,
            inseminationEvent.AiDate,
            inseminationEvent.ExpectedFarrowDate,
            inseminationEvent.SynchronizationEventId);

        public void AddInseminationEvent( InseminationEvent inseminationEvent )
        {
            using (_lock.Write())
            {
                _inseminationEvents.Add(inseminationEvent);
                OnInseminationEventAdded?.Invoke(inseminationEvent);
            }
        }

        public void UpdateInseminationEvent( InseminationEvent inseminationEvent )
        {
            using (_lock.Write())
            {
                int index = _inseminationEvents.FindIndex(e => e.BreedingEventId == inseminationEvent.BreedingEventId);
                if (index != -1)
                {
                    _inseminationEvents[index] = inseminationEvent;
                    OnInseminationEventUpdated?.Invoke(inseminationEvent);
                }
            }
        }

        public void DeleteInseminationEvent( int id )
        {
            using (_lock.Write())
            {
                InseminationEvent? inseminationEvent = _inseminationEvents.FirstOrDefault(e => e.BreedingEventId == id);
                if (inseminationEvent == null) return;
                _inseminationEvents.Remove(inseminationEvent);
                OnInseminationEventDeleted?.Invoke(inseminationEvent);
            }
        }

        public event Action? OnInseminationEventsLoaded;
        public event Action<InseminationEvent>? OnInseminationEventAdded;
        public event Action<InseminationEvent>? OnInseminationEventUpdated;
        public event Action<InseminationEvent>? OnInseminationEventDeleted;

        private string? GetAnimalNameForInsemination( int animalId ) => animalStore.GetAnimalById(animalId)?.Name.ToString();
    }
}
