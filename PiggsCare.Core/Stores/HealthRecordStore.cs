using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;

namespace PiggsCare.Core.Stores
{
    public class HealthRecordStore:IHealthRecordStore
    {
        #region Constructor

        public HealthRecordStore( IHealthRecordService healthRecordService )
        {
            _healthRecordService = healthRecordService;
            _initializeTask = new Lazy<Task>(() => Init());
        }

        #endregion

        #region Properties

        public IEnumerable<HealthRecord> HealthRecords => _healthRecords;

        #endregion

        #region CRUD Operations

        public async Task Create( HealthRecord healthRecord )
        {
            await _healthRecordService.CreateHealthRecordAsync(healthRecord);
            _healthRecords.Add(healthRecord);
            OnSave?.Invoke(healthRecord);
        }

        public async Task Modify( HealthRecord healthRecord )
        {
            await _healthRecordService.UpdateHealthRecordAsync(healthRecord);
            int currentIndex = _healthRecords.FindIndex(x => x.HealthRecordId == healthRecord.HealthRecordId);

            if (currentIndex != -1)
            {
                _healthRecords[currentIndex] = healthRecord;
            }
            else
            {
                _healthRecords.Add(healthRecord);
            }
            OnUpdate?.Invoke(healthRecord);
        }

        public async Task Remove( int id )
        {
            await _healthRecordService.DeleteHealthRecordAsync(id);
            _healthRecords.RemoveAll(x => x.HealthRecordId == id);
            OnDelete?.Invoke(id);
        }

        #endregion

        #region Events

        public event Action? OnLoad;
        public event Action<HealthRecord>? OnSave;
        public event Action<HealthRecord>? OnUpdate;
        public event Action<int>? OnDelete;

        #endregion

        #region Methods

        public async Task Load( int id )
        {
            try
            {
                _initializeTask = new Lazy<Task>(Init(id));
                await _initializeTask.Value;
            }
            catch (Exception)
            {
                _initializeTask = new Lazy<Task>(Init(id));
                throw;
            }
            finally
            {
                OnLoad?.Invoke();
            }
        }

        private async Task Init( int id = 1 )
        {
            _healthRecords.Clear();
            IEnumerable<HealthRecord> healthRecords = await _healthRecordService.GetAllHealthRecordsAsync(id);
            _healthRecords.AddRange(healthRecords);
        }

        #endregion

        #region Fields

        private readonly List<HealthRecord> _healthRecords = [];
        private readonly IHealthRecordService _healthRecordService;
        private Lazy<Task> _initializeTask;
        private int _animalId;

        #endregion
    }
}
