using PiggsCare.Domain.Models;

namespace PiggsCare.ApplicationState.Stores.Removal
{
    public interface IRemovalEventStore
    {
        IEnumerable<RemovalEvent> RemovalEvents { get; }

        void LoadRemovalEvents( IEnumerable<RemovalEvent> events );

        RemovalEvent? GetRemovalEventById( int id );

        RemovalEvent CreateRemovalEventWithCorrectId( int removalEventId, RemovalEvent removalEvent );

        void AddRemovalEvent( RemovalEvent removalEvent );

        void UpdateRemovalEvent( RemovalEvent removalEvent );

        void DeleteRemovalEvent( int id );

        event Action? OnRemovalEventsLoaded;
        event Action<RemovalEvent>? OnRemovalEventAdded;
        event Action<RemovalEvent>? OnRemovalEventUpdated;
        event Action<RemovalEvent>? OnRemovalEventDeleted;
    }
}
