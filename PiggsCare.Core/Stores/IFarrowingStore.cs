using PiggsCare.Domain.Models;

namespace PiggsCare.Core.Stores
{
    public interface IFarrowingStore
    {
        IEnumerable<FarrowEvent> FarrowEvents { get; }

        Task Load( int id );

        Task Create( FarrowEvent farrowEvent );

        Task Modify( FarrowEvent farrow );

        Task Remove( int id );

        event Action OnLoad;
        event Action<FarrowEvent> OnSave;
        event Action<FarrowEvent> OnUpdate;
        event Action<int> OnDelete;
    }
}
