using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;

namespace PiggsCare.Core.Stores
{
    public class BreedingEventStore:IBreedingEventStore
    {
        #region Construtor

        public BreedingEventStore( IBreedingEventService breedingEventService )
        {
            _breedingEventService = breedingEventService;
            _initializeTask = new Lazy<Task>(() => Init());
        }

        #endregion

        #region Properties

        public IEnumerable<BreedingEvent> BreedingEvents => _breedingEvents;

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

        #endregion

        #region Methods

        private async Task Init( int id = 1 )
        {
            _breedingEvents.Clear();
            IEnumerable<BreedingEvent> breedingEvents = await _breedingEventService.GetAllBreedingEventsAsync(id);
            _breedingEvents.AddRange(breedingEvents);
        }

        public async Task Load( int id )
        {
            try
            {
                _initializeTask = new Lazy<Task>(() => Init(id));
                await _initializeTask.Value;
            }
            catch (Exception e)
            {
                _initializeTask = new Lazy<Task>(() => Init(id));
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
        private readonly IBreedingEventService _breedingEventService;
        private Lazy<Task> _initializeTask;
        private int _animalId;

        #endregion
    }
}
