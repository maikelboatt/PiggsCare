using PiggsCare.Domain.Models;

namespace PiggsCare.ApplicationState.Stores.Synchronization
{
    public interface ISynchronizationEventStore
    {
        IEnumerable<SynchronizationEvent> SynchronizationEvents { get; }

        void LoadSynchronizationEvents( IEnumerable<SynchronizationEvent> events );

        SynchronizationEvent? GetSynchronizationEventById( int id );

        SynchronizationEvent CreateSynchronizationEventWithCorrectId( int synchronizationId, SynchronizationEvent synchronizationEvent );

        void AddSynchronizationEvent( SynchronizationEvent synchronizationEvent );

        void UpdateSynchronizationEvent( SynchronizationEvent synchronizationEvent );

        void DeleteSynchronizationEvent( int id );

        event Action? OnSynchronizationEventsLoaded;
        event Action<SynchronizationEvent>? OnSynchronizationEventAdded;
        event Action<SynchronizationEvent>? OnSynchronizationEventUpdated;
        event Action<SynchronizationEvent>? OnSynchronizationEventDeleted;
    }
}
