using PiggsCare.Domain.Models;

namespace PiggsCare.ApplicationState.Stores.Health
{
    public interface IHealthRecordStore
    {
        IEnumerable<HealthRecord> HealthRecords { get; }

        void LoadHealthRecords( IEnumerable<HealthRecord> healthRecords, CancellationToken cancellationToken = default );

        HealthRecord? GetHealthRecordById( int healthRecordId );

        HealthRecord CreateHealthRecordWithCorrectId( int healthRecordId, HealthRecord healthRecord );

        void AddHealthRecord( HealthRecord healthRecord );

        void UpdateHealthRecord( HealthRecord healthRecord );

        void DeleteHealthRecord( int healthRecordId );

        event Action? OnHealthRecordsLoaded;
        event Action<HealthRecord>? OnHealthRecordAdded;
        event Action<HealthRecord>? OnHealthRecordUpdated;
        event Action<HealthRecord>? OnHealthRecordDeleted;
    }
}
