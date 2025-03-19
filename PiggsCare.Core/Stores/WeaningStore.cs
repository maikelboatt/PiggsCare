using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;

namespace PiggsCare.Core.Stores
{
    public class WeaningStore:IWeaningStore
    {
        #region Constructor

        public WeaningStore( IWeaningService weaningService )
        {
            _weaningService = weaningService;
            _initializeTask = new Lazy<Task>(() => Init());
        }

        #endregion

        #region Properties

        public IEnumerable<WeaningEvent> WeaningEvents => _weaningEvents;

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
            _weaningEvents.Clear();
            IEnumerable<WeaningEvent> weaningEvents = await _weaningService.GetAllWeaningEventAsync(id);
            _weaningEvents.AddRange(weaningEvents);
        }

        #endregion

        #region CRUD Operations

        public async Task Create( WeaningEvent weaning )
        {
            await _weaningService.CreateWeaningEventAsync(weaning);
            _weaningEvents.Add(weaning);
            OnSave?.Invoke(weaning);
        }

        public async Task Modify( WeaningEvent weaning )
        {
            await _weaningService.UpdateWeaningEventAsync(weaning);
            int currentIndex = _weaningEvents.FindIndex(x => x.WeaningEventId == weaning.WeaningEventId);

            if (currentIndex != -1)
            {
                _weaningEvents[currentIndex] = weaning;
            }
            else
            {
                _weaningEvents.Add(weaning);
            }
            OnUpdate?.Invoke(weaning);
        }

        public async Task Remove( int id )
        {
            await _weaningService.DeleteWeaningEventAsync(id);
            _weaningEvents.RemoveAll(x => x.WeaningEventId == id);
            OnDelete?.Invoke(id);
        }

        #endregion

        #region Fields

        private readonly IWeaningService _weaningService;
        private readonly List<WeaningEvent> _weaningEvents = [];
        private Lazy<Task> _initializeTask;

        #endregion

        #region Events

        public event Action? OnLoad;
        public event Action<WeaningEvent>? OnSave;
        public event Action<WeaningEvent>? OnUpdate;
        public event Action<int>? OnDelete;

        #endregion
    }
}
