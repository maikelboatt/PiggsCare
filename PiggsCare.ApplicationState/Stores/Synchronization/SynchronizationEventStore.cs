using Microsoft.Extensions.Logging;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Helpers;

namespace PiggsCare.ApplicationState.Stores.Synchronization
{
    public class SynchronizationEventStore( ILogger<SynchronizationEventStore> logger ):ISynchronizationEventStore
    {
        private readonly ReaderWriterLockSlim _lock = new();
        private readonly ILogger<SynchronizationEventStore> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly List<SynchronizationEvent> _synchronizationEvents = [];

        public IEnumerable<SynchronizationEvent> SynchronizationEvents
        {
            get
            {
                using (_lock.Read())
                {
                    return _synchronizationEvents.ToList();
                }
            }
        }

        public void LoadSynchronizationEvents( IEnumerable<SynchronizationEvent> events )
        {
            try
            {
                using (_lock.Write())
                {
                    _synchronizationEvents.Clear();
                    _synchronizationEvents.AddRange(events);
                }

                OnSynchronizationEventsLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error loading synchronization events: {ExMessage}", ex.Message);
                throw;
            }
        }

        public SynchronizationEvent? GetSynchronizationEventById( int id )
        {
            using (_lock.Read())
            {
                return _synchronizationEvents.FirstOrDefault(e => e.SynchronizationEventId == id);
            }
        }

        public SynchronizationEvent CreateSynchronizationEventWithCorrectId( int synchronizationId, SynchronizationEvent synchronizationEvent ) => new(
            synchronizationId,
            synchronizationEvent.StartDate,
            synchronizationEvent.EndDate,
            synchronizationEvent.BatchNumber,
            synchronizationEvent.SynchronizationProtocol,
            synchronizationEvent.Comments);

        public void AddSynchronizationEvent( SynchronizationEvent synchronizationEvent )
        {
            using (_lock.Write())
            {
                _synchronizationEvents.Add(synchronizationEvent);
            }
            OnSynchronizationEventAdded?.Invoke(synchronizationEvent);
        }

        public void UpdateSynchronizationEvent( SynchronizationEvent synchronizationEvent )
        {
            using (_lock.Write())
            {
                int index = _synchronizationEvents.FindIndex(e => e.SynchronizationEventId == synchronizationEvent.SynchronizationEventId);
                if (index != -1)
                {
                    _synchronizationEvents[index] = synchronizationEvent;
                }
            }
            OnSynchronizationEventUpdated?.Invoke(synchronizationEvent);
        }

        public void DeleteSynchronizationEvent( int id )
        {
            SynchronizationEvent? deleted = null;

            using (_lock.Write())
            {
                deleted = GetSynchronizationEventById(id);
                if (deleted != null)
                {
                    _synchronizationEvents.Remove(deleted);
                }
            }

            if (deleted != null)
            {
                OnSynchronizationEventDeleted?.Invoke(deleted);
            }
        }

        public event Action? OnSynchronizationEventsLoaded;
        public event Action<SynchronizationEvent>? OnSynchronizationEventAdded;
        public event Action<SynchronizationEvent>? OnSynchronizationEventUpdated;
        public event Action<SynchronizationEvent>? OnSynchronizationEventDeleted;
    }
}
