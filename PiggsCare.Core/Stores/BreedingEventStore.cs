using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;

namespace PiggsCare.Core.Stores
{
    public class BreedingEventStore:IBreedingEventStore
    {
        #region Constructor

        public BreedingEventStore( IBreedingEventService breedingEventService )
        {
            _breedingEventService = breedingEventService;
            _initializeTask = new Lazy<Task>(() => InitForAnimal());
        }

        #endregion

        #region Properties

        public IEnumerable<BreedingEvent> BreedingEvents => _breedingEvents;
        public IEnumerable<BreedingEventWithAnimal> BreedingEventsBatch => _breedingEventsBatch;

        #endregion

        #region CRUD Operations

        public async Task Create( BreedingEvent breedingEvent )
        {
            await _breedingEventService.CreateBreedingEventAsync(breedingEvent);
            _breedingEvents.Add(breedingEvent);
            OnSave?.Invoke(breedingEvent);
        }

        public async Task Modify( BreedingEvent breedingEvent )
        {
            await _breedingEventService.UpdateBreedingEventAsync(breedingEvent);
            int currentIndex = _breedingEvents.FindIndex(x => x.BreedingEventId == breedingEvent.BreedingEventId);

            if (currentIndex != -1)
            {
                _breedingEvents[currentIndex] = breedingEvent;
            }
            else
            {
                _breedingEvents.Add(breedingEvent);
            }
            OnUpdate?.Invoke(breedingEvent);
        }

        public async Task Remove( int id )
        {
            await _breedingEventService.DeleteBreedingEventAsync(id);
            _breedingEvents.RemoveAll(x => x.BreedingEventId == id);
            OnDelete?.Invoke(id);
        }

        public async Task<BreedingEvent> GetUnique( int id )
        {
            BreedingEvent? result = await _breedingEventService.GetBreedingEventByIdAsync(id);
            return result;

        }

        #endregion

        #region Methods

        private async Task InitForAnimal( int id = 1 )
        {
            _breedingEvents.Clear();
            IEnumerable<BreedingEvent> breedingEvents = await _breedingEventService.GetAllBreedingEventsAsync(id);
            _breedingEvents.AddRange(breedingEvents);
        }

        private async Task InitForBatch( int id = 1 )
        {
            _breedingEventsBatch.Clear();
            IEnumerable<BreedingEventWithAnimal> breedingEvents = await _breedingEventService.GetAllBreedingEventBySynchronizationBatchAsync(id);
            _breedingEventsBatch.AddRange(breedingEvents);
        }

        public async Task LoadForAnimal( int id )
        {
            try
            {
                _initializeTask = new Lazy<Task>(() => InitForAnimal(id));
                await _initializeTask.Value;
            }
            catch (Exception e)
            {
                _initializeTask = new Lazy<Task>(() => InitForAnimal(id));
                throw;
            }
            finally
            {
                OnLoad?.Invoke();
            }
        }

        public async Task LoadForBatch( int id )
        {
            try
            {
                _initializeTask = new Lazy<Task>(() => InitForBatch(id));
                await _initializeTask.Value;
            }
            catch (Exception e)
            {
                _initializeTask = new Lazy<Task>(() => InitForBatch(id));
                throw;
            }
            finally
            {
                OnLoad?.Invoke();
            }
        }

        #endregion

        #region Events

        public event Action? OnLoad;
        public event Action<BreedingEvent>? OnSave;
        public event Action<BreedingEvent>? OnUpdate;
        public event Action<int>? OnDelete;

        #endregion

        #region Fields

        private readonly List<BreedingEvent> _breedingEvents = [];
        private readonly List<BreedingEventWithAnimal> _breedingEventsBatch = [];
        private readonly IBreedingEventService _breedingEventService;
        private Lazy<Task> _initializeTask;
        private int _animalId;

        #endregion
    }
}
