using PiggsCare.Domain.Models;

namespace PiggsCare.Core.Stores
{
    public interface IRemovalEventStore
    {
        IEnumerable<RemovalEvent> RemovalEvents { get; }

        Task LoadAsync( int id );

        Task CreateAsync( RemovalEvent scan );

        Task ModifyAsync( RemovalEvent scan );

        Task RemoveAsync( int id );

        event Action OnLoad;
        event Action<RemovalEvent> OnSave;
        event Action<RemovalEvent> OnUpdate;
        event Action<int> OnDelete;
    }
}
