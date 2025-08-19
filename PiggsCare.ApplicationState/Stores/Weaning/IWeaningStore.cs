using PiggsCare.Domain.Models;

namespace PiggsCare.ApplicationState.Stores.Weaning
{
    public interface IWeaningStore
    {
        IEnumerable<WeaningEvent> WeaningEvents { get; }

        void LoadWeaningEvents( IEnumerable<WeaningEvent> events );

        WeaningEvent? GetWeaningEventById( int id );

        WeaningEvent CreateWeaningEventWithCorrectId( int weaningEventId, WeaningEvent weaningEvent );

        void AddWeaningEvent( WeaningEvent weaningEvent );

        void UpdateWeaningEvent( WeaningEvent weaningEvent );

        void DeleteWeaningEvent( int id );

        event Action? OnWeaningEventsLoaded;
        event Action<WeaningEvent>? OnWeaningEventAdded;
        event Action<WeaningEvent>? OnWeaningEventUpdated;
        event Action<WeaningEvent>? OnWeaningEventDeleted;
    }
}
