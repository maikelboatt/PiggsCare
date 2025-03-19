using PiggsCare.Domain.Models;

namespace PiggsCare.Core.Stores
{
    public interface ISynchronizationEventStore
    {
        IEnumerable<SynchronizationEvent> SynchronizationEvents { get; }

        Task LoadAsync();

        Task<int> CreateAsync( SynchronizationEvent synchronizationEvent );

        Task ModifyAsync( SynchronizationEvent synchronizationEvent );

        Task RemoveAsync( int id );

        event Action OnLoad;
        event Action<SynchronizationEvent> OnSave;
        event Action<SynchronizationEvent> OnUpdate;
        event Action<int> OnDelete;
    }
}
