using Microsoft.Extensions.Logging;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Helpers;

namespace PiggsCare.ApplicationState.Stores.Removal
{
    public class RemovalEventStore( ILogger<RemovalEventStore> logger ):IRemovalEventStore
    {
        private readonly ReaderWriterLockSlim _lock = new();
        private readonly ILogger<RemovalEventStore> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly List<RemovalEvent> _removalEvents = [];

        public IEnumerable<RemovalEvent> RemovalEvents
        {
            get
            {
                using (_lock.Read())
                {
                    return _removalEvents.ToList();
                }
            }
        }

        public void LoadRemovalEvents( IEnumerable<RemovalEvent> events )
        {
            try
            {
                using (_lock.Write())
                {
                    _removalEvents.Clear();
                    _removalEvents.AddRange(events);
                }

                OnRemovalEventsLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error loading removal events: {ExMessage}", ex.Message);
                throw;
            }
        }

        public RemovalEvent? GetRemovalEventById( int id )
        {
            using (_lock.Read())
            {
                return _removalEvents.FirstOrDefault(e => e.RemovalEventId == id);
            }
        }

        public RemovalEvent CreateRemovalEventWithCorrectId( int removalEventId, RemovalEvent removalEvent ) => new(
            removalEventId,
            removalEvent.AnimalId,
            removalEvent.RemovalDate,
            removalEvent.ReasonForRemoval);


        public void AddRemovalEvent( RemovalEvent removalEvent )
        {
            using (_lock.Write())
            {
                _removalEvents.Add(removalEvent);
                OnRemovalEventAdded?.Invoke(removalEvent);
            }
        }

        public void UpdateRemovalEvent( RemovalEvent removalEvent )
        {
            using (_lock.Write())
            {
                int index = _removalEvents.FindIndex(e => e.RemovalEventId == removalEvent.RemovalEventId);
                if (index == -1) return;
                _removalEvents[index] = removalEvent;
                OnRemovalEventUpdated?.Invoke(removalEvent);
            }
        }

        public void DeleteRemovalEvent( int id )
        {
            using (_lock.Write())
            {
                RemovalEvent? removalEvent = _removalEvents.FirstOrDefault(e => e.RemovalEventId == id);
                if (removalEvent == null) return;

                _removalEvents.Remove(removalEvent);
                OnRemovalEventDeleted?.Invoke(removalEvent);
            }
        }

        public event Action? OnRemovalEventsLoaded;
        public event Action<RemovalEvent>? OnRemovalEventAdded;
        public event Action<RemovalEvent>? OnRemovalEventUpdated;
        public event Action<RemovalEvent>? OnRemovalEventDeleted;
    }
}
