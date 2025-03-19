using PiggsCare.Domain.Models;

namespace PiggsCare.Core.Stores
{
    public interface IPregnancyStore
    {
        IEnumerable<PregnancyScan> PregnancyScans { get; }

        Task Load( int id );

        Task Create( PregnancyScan scan );

        Task Modify( PregnancyScan scan );

        Task Remove( int id );

        event Action OnLoad;
        event Action<PregnancyScan> OnSave;
        event Action<PregnancyScan> OnUpdate;
        event Action<int> OnDelete;
    }
}
