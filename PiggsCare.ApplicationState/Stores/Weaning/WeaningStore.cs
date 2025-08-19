using Microsoft.Extensions.Logging;
using PiggsCare.Domain.Models;

namespace PiggsCare.ApplicationState.Stores.Weaning
{
    public class WeaningStore( ILogger<WeaningStore> logger ):IWeaningStore
    {
        private readonly ReaderWriterLockSlim _lock = new();
        private readonly ILogger<WeaningStore> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly List<WeaningEvent> _weaningEvents = [];

        public IEnumerable<WeaningEvent> WeaningEvents
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _weaningEvents.ToList();
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }

        public void LoadWeaningEvents( IEnumerable<WeaningEvent> events )
        {
            try
            {
                _lock.EnterWriteLock();
                try
                {
                    _weaningEvents.Clear();
                    _weaningEvents.AddRange(events);
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
                OnWeaningEventsLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error loading weaning events: {ExMessage}", ex.Message);
                throw;
            }
        }

        public WeaningEvent? GetWeaningEventById( int id )
        {
            _lock.EnterReadLock();
            try
            {
                return _weaningEvents.FirstOrDefault(e => e.WeaningEventId == id);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public WeaningEvent CreateWeaningEventWithCorrectId( int weaningEventId, WeaningEvent weaningEvent ) => new(
            weaningEventId,
            weaningEvent.FarrowingEventId,
            weaningEvent.WeaningDate,
            weaningEvent.NumberWeaned,
            weaningEvent.MalesWeaned,
            weaningEvent.FemalesWeaned,
            weaningEvent.AverageWeaningWeight);

        public void AddWeaningEvent( WeaningEvent weaningEvent )
        {
            _lock.EnterWriteLock();
            try
            {
                _weaningEvents.Add(weaningEvent);
                OnWeaningEventAdded?.Invoke(weaningEvent);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void UpdateWeaningEvent( WeaningEvent weaningEvent )
        {
            _lock.EnterWriteLock();
            try
            {
                int index = _weaningEvents.FindIndex(e => e.WeaningEventId == weaningEvent.WeaningEventId);
                if (index != -1)
                {
                    _weaningEvents[index] = weaningEvent;
                    OnWeaningEventUpdated?.Invoke(weaningEvent);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void DeleteWeaningEvent( int id )
        {
            _lock.EnterWriteLock();
            try
            {
                WeaningEvent? weaningEvent = GetWeaningEventById(id);
                if (weaningEvent == null) return;
                _weaningEvents.Remove(weaningEvent);
                OnWeaningEventDeleted?.Invoke(weaningEvent);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public event Action? OnWeaningEventsLoaded;
        public event Action<WeaningEvent>? OnWeaningEventAdded;
        public event Action<WeaningEvent>? OnWeaningEventUpdated;
        public event Action<WeaningEvent>? OnWeaningEventDeleted;
    }
}
