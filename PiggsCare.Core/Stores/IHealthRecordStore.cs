using PiggsCare.Domain.Models;

namespace PiggsCare.Core.Stores
{
    public interface IHealthRecordStore
    {
        IEnumerable<HealthRecord> HealthRecords { get; }

        Task Load( int id );

        Task Create( HealthRecord healthRecord );

        Task Modify( HealthRecord healthRecord );

        Task Remove( int id );

        event Action OnLoad;
        event Action<HealthRecord> OnSave;
        event Action<HealthRecord> OnUpdate;
        event Action<int> OnDelete;
    }
}
