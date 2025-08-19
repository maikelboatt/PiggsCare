using PiggsCare.Domain.Models;

namespace PiggsCare.ApplicationState.Stores.Insemination
{
    public interface IInseminationEventStore
    {
        IEnumerable<InseminationEvent> InseminationEvents { get; }
        IEnumerable<InseminationEventWithAnimal> InseminationEventsWithAnimals { get; }

        void LoadInseminationEvents( IEnumerable<InseminationEvent> events, CancellationToken cancellationToken = default );

        InseminationEvent? GetInseminationEventById( int id );

        InseminationEvent? GetInseminationEventBySynchronizationId( int synchronizationId );

        void GetAllInseminationEventsBySynchronizationBatch( int synchronizationId );

        InseminationEvent CreateInseminationEventWithCorrectId( int inseminationEventId, InseminationEvent inseminationEvent );

        void AddInseminationEvent( InseminationEvent inseminationEvent );

        void UpdateInseminationEvent( InseminationEvent inseminationEvent );

        void DeleteInseminationEvent( int id );

        event Action? OnInseminationEventsLoaded;
        event Action<InseminationEvent>? OnInseminationEventAdded;
        event Action<InseminationEvent>? OnInseminationEventUpdated;
        event Action<InseminationEvent>? OnInseminationEventDeleted;
    }
}
