using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;

namespace PiggsCare.Core.Stores
{
    public class SynchronizationEventStore:ISynchronizationEventStore
    {
        #region Constructor

        public SynchronizationEventStore( ISynchronizationEventService synchronizationEventService )
        {
            _synchronizationEventService = synchronizationEventService;
            _initializeTask = new Lazy<Task>(() => InitializeAsync());
        }

        #endregion

        #region Fields

        private readonly ISynchronizationEventService _synchronizationEventService;
        private readonly List<SynchronizationEvent> _synchronizationEvents = [];
        private Lazy<Task>? _initializeTask;

        #endregion

        #region Properties

        public IEnumerable<SynchronizationEvent> SynchronizationEvents => _synchronizationEvents;
        public bool IsLoading { get; private set; }

        #endregion

        #region CRUD Operations

        public async Task<int> CreateAsync( SynchronizationEvent synchronizationEvent )
        {

            int result;
            IsLoading = true;
            try
            {
                result = await _synchronizationEventService.CreateSynchronizationEventAsync(synchronizationEvent);
                _synchronizationEvents.Add(synchronizationEvent);
                OnSave?.Invoke(synchronizationEvent);
            }
            finally
            {
                IsLoading = false;
            }

            return result;
        }

        public async Task ModifyAsync( SynchronizationEvent synchronizationEvent )
        {

            IsLoading = true;
            try
            {
                await _synchronizationEventService.UpdateSynchronizationEventAsync(synchronizationEvent);
                int currentIndex = _synchronizationEvents.FindIndex(x => x.SynchronizationEventId == synchronizationEvent.SynchronizationEventId);

                if (currentIndex != -1)
                {
                    _synchronizationEvents[currentIndex] = synchronizationEvent;
                }
                else
                {
                    _synchronizationEvents.Add(synchronizationEvent);
                }
                OnUpdate?.Invoke(synchronizationEvent);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task RemoveAsync( int synchronizationEventId )
        {
            IsLoading = true;
            try
            {
                await _synchronizationEventService.DeleteSynchronizationEventAsync(synchronizationEventId);
                _synchronizationEvents.RemoveAll(x => x.SynchronizationEventId == synchronizationEventId);
                OnDelete?.Invoke(synchronizationEventId);
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        #region Load Operations

        public async Task LoadAsync()
        {
            IsLoading = true;
            try
            {
                _initializeTask = new Lazy<Task>(InitializeAsync());
                await _initializeTask.Value;
            }
            catch (Exception)
            {
                _initializeTask = new Lazy<Task>(InitializeAsync());
                throw;
            }
            finally
            {
                OnLoad?.Invoke();
            }
        }

        private async Task InitializeAsync()
        {
            _synchronizationEvents.Clear();
            IEnumerable<SynchronizationEvent> events = await _synchronizationEventService.GetAllSynchronizationEventsAsync();
            _synchronizationEvents.AddRange(events);
        }

        #endregion

        #region Events

        public event Action? OnLoad;
        public event Action<SynchronizationEvent>? OnSave;
        public event Action<SynchronizationEvent>? OnUpdate;
        public event Action<int>? OnDelete;

        #endregion
    }
}
