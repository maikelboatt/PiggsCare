using PiggsCare.Domain.Models;

namespace PiggsCare.Core.Stores
{
    public interface IWeaningStore
    {
        IEnumerable<WeaningEvent> WeaningEvents { get; }

        Task Load( int id );

        Task Create( WeaningEvent weaning );

        Task Modify( WeaningEvent weaning );

        Task Remove( int id );

        event Action OnLoad;
        event Action<WeaningEvent> OnSave;
        event Action<WeaningEvent> OnUpdate;
        event Action<int> OnDelete;
    }
}
