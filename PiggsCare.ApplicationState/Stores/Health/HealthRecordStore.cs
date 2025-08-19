using Microsoft.Extensions.Logging;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Helpers;

namespace PiggsCare.ApplicationState.Stores.Health
{
    public class HealthRecordStore( ILogger<HealthRecordStore> logger ):IHealthRecordStore
    {
        private readonly List<HealthRecord> _healthRecords = [];
        private readonly ReaderWriterLockSlim _lock = new();
        private readonly ILogger<HealthRecordStore> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public IEnumerable<HealthRecord> HealthRecords
        {
            get
            {
                using (_lock.Read())
                {
                    return _healthRecords.ToList();
                }
            }
        }

        public void LoadHealthRecords( IEnumerable<HealthRecord> healthRecords, CancellationToken cancellationToken = default )
        {
            try
            {
                using (_lock.Write())
                {
                    _healthRecords.Clear();
                    _healthRecords.AddRange(healthRecords);
                }

                OnHealthRecordsLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error loading health records: {ExMessage}", ex.Message);
                throw;
            }
        }

        public HealthRecord? GetHealthRecordById( int healthRecordId )
        {
            using (_lock.Read())
            {
                return _healthRecords.FirstOrDefault(hr => hr.HealthRecordId == healthRecordId);
            }

        }

        public void AddHealthRecord( HealthRecord healthRecord )
        {
            using (_lock.Write())
            {
                _healthRecords.Add(healthRecord);
                OnHealthRecordAdded?.Invoke(healthRecord);
            }
        }

        public HealthRecord CreateHealthRecordWithCorrectId( int healthRecordId, HealthRecord healthRecord ) => new(
            healthRecordId,
            healthRecord.AnimalId,
            healthRecord.RecordDate,
            healthRecord.Diagnosis,
            healthRecord.Treatment,
            healthRecord.Outcome);

        public void UpdateHealthRecord( HealthRecord healthRecord )
        {
            using (_lock.Write())
            {
                int index = _healthRecords.FindIndex(hr => hr.HealthRecordId == healthRecord.HealthRecordId);
                if (index == -1) return;
                _healthRecords[index] = healthRecord;
                OnHealthRecordUpdated?.Invoke(healthRecord);
            }
        }

        public void DeleteHealthRecord( int healthRecordId )
        {
            using (_lock.Write())
            {
                HealthRecord? healthRecord = _healthRecords.FirstOrDefault(hr => hr.HealthRecordId == healthRecordId);
                if (healthRecord == null) return;
                _healthRecords.Remove(healthRecord);
                OnHealthRecordDeleted?.Invoke(healthRecord);
            }
        }

        public event Action? OnHealthRecordsLoaded;
        public event Action<HealthRecord>? OnHealthRecordAdded;
        public event Action<HealthRecord>? OnHealthRecordUpdated;
        public event Action<HealthRecord>? OnHealthRecordDeleted;
    }
}
