using PiggsCare.Domain.Models;

namespace PiggsCare.ApplicationState.Stores.Farrowing
{
    public interface IFarrowingStore
    {
        IEnumerable<FarrowEvent> Farrowings { get; }

        void LoadFarrowingEvent( IEnumerable<FarrowEvent> farrowings, CancellationToken cancellationToken = default );

        FarrowEvent CreateFarrowingWithCorrectId( int farrowingId, FarrowEvent farrowing );

        FarrowEvent? GetFarrowingByAnimalId( int farrowEventId );

        FarrowEvent? GetFarrowingById( int farrowingId );

        void AddFarrowing( FarrowEvent farrowing );

        void UpdateFarrowing( FarrowEvent farrowing );

        void DeleteFarrowing( int farrowingId );

        event Action? OnFarrowingsLoaded;
        event Action<FarrowEvent>? OnFarrowingAdded;
        event Action<FarrowEvent>? OnFarrowingUpdated;
        event Action<FarrowEvent>? OnFarrowingDeleted;
    }
}
