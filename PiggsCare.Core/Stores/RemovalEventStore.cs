using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;

namespace PiggsCare.Core.Stores
{
    public class RemovalEventStore:IRemovalEventStore
    {
        #region Constructor

        public RemovalEventStore( IRemovalEventService removalEventService )
        {
            _removalEventService = removalEventService;
            _initializeTask = new Lazy<Task>(() => InitializeAsync());
        }

        #endregion

        #region Fields

        private readonly IRemovalEventService _removalEventService;
        private readonly List<RemovalEvent> _removalEvents = [];
        private Lazy<Task>? _initializeTask;

        #endregion

        #region Properties

        public IEnumerable<RemovalEvent> RemovalEvents => _removalEvents;
        public bool IsLoading { get; private set; }

        #endregion

        #region CRUD Operations

        public async Task CreateAsync( RemovalEvent removalEvent )
        {
            IsLoading = true;
            try
            {
                await _removalEventService.CreateRemovalEventAsync(removalEvent);
                _removalEvents.Add(removalEvent);
                OnSave?.Invoke(removalEvent);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task ModifyAsync( RemovalEvent removalEvent )
        {
            IsLoading = true;
            try
            {
                await _removalEventService.UpdateRemovalEventAsync(removalEvent);
                int currentIndex = _removalEvents.FindIndex(x => x.RemovalEventId == removalEvent.RemovalEventId);

                if (currentIndex != -1)
                {
                    _removalEvents[currentIndex] = removalEvent;
                }
                else
                {
                    _removalEvents.Add(removalEvent);
                }
                OnUpdate?.Invoke(removalEvent);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task RemoveAsync( int removalEventId )
        {
            IsLoading = true;
            try
            {
                await _removalEventService.DeleteRemovalEventAsync(removalEventId);
                _removalEvents.RemoveAll(x => x.RemovalEventId == removalEventId);
                OnDelete?.Invoke(removalEventId);
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        #region Load Operations

        public async Task LoadAsync( int animalId )
        {
            IsLoading = true;
            try
            {
                _initializeTask = new Lazy<Task>(InitializeAsync(animalId));
                await _initializeTask.Value;
            }
            catch (Exception)
            {
                _initializeTask = new Lazy<Task>(InitializeAsync(animalId));
                throw;
            }
            finally
            {
                OnLoad?.Invoke();
            }
        }

        private async Task InitializeAsync( int animalId = 0 )
        {
            _removalEvents.Clear();
            IEnumerable<RemovalEvent> events;
            if (animalId > 0)
            {
                events = await _removalEventService.GetAllRemovalEventsAsync(animalId);
            }
            else
            {
                events = Enumerable.Empty<RemovalEvent>();
            }
            _removalEvents.AddRange(events);
        }

        #endregion

        #region Events

        public event Action? OnLoad;
        public event Action<RemovalEvent>? OnSave;
        public event Action<RemovalEvent>? OnUpdate;
        public event Action<int>? OnDelete;

        #endregion
    }
}
