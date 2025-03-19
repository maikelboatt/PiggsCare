using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;

namespace PiggsCare.Core.Stores
{
    public class FarrowingStore:IFarrowingStore
    {
        #region Constructor

        public FarrowingStore( IFarrowingService farrowingService )
        {
            _farrowingService = farrowingService;
            _initializeTask = new Lazy<Task>(() => Init());
        }

        #endregion

        #region Properites

        public IEnumerable<FarrowEvent> FarrowEvents => _farrowEvents;

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
            _farrowEvents.Clear();
            IEnumerable<FarrowEvent> farrowEvents = await _farrowingService.GetAllFarrowEventAsync(id);
            _farrowEvents.AddRange(farrowEvents);
        }

        #endregion

        #region CRUD Operations

        public async Task Create( FarrowEvent farrowEvent )
        {
            await _farrowingService.CreateFarrowEventAsync(farrowEvent);
            _farrowEvents.Add(farrowEvent);
            OnSave?.Invoke(farrowEvent);
        }

        public async Task Modify( FarrowEvent farrow )
        {
            await _farrowingService.UpdateFarrowEventAsync(farrow);
            int currentIndex = _farrowEvents.FindIndex(x => x.FarrowingEventId == farrow.FarrowingEventId);

            if (currentIndex != -1)
            {
                _farrowEvents[currentIndex] = farrow;
            }
            else
            {
                _farrowEvents.Add(farrow);
            }
            OnUpdate?.Invoke(farrow);
        }

        public async Task Remove( int id )
        {
            await _farrowingService.DeleteFarrowEventAsync(id);
            _farrowEvents.RemoveAll(x => x.FarrowingEventId == id);
            OnDelete?.Invoke(id);
        }

        #endregion

        #region Fields

        private readonly IFarrowingService _farrowingService;
        private readonly List<FarrowEvent> _farrowEvents = [];
        private Lazy<Task> _initializeTask;

        #endregion

        #region Events

        public event Action? OnLoad;
        public event Action<FarrowEvent>? OnSave;
        public event Action<FarrowEvent>? OnUpdate;
        public event Action<int>? OnDelete;

        #endregion
    }
}
