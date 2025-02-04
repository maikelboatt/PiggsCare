using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;

namespace PiggsCare.Core.Stores
{
    public class AnimalStore:IAnimalStore
    {
        #region Constructor

        public AnimalStore( IAnimalService animalService )
        {
            _animalService = animalService;
            _initializeTask = new Lazy<Task>(Init);
        }

        #endregion

        #region Properties

        public IEnumerable<Animal> Animals => _animals;

        #endregion

        #region CRUD Operations

        public async Task Create( Animal animal )
        {
            await _animalService.CreateAnimalAsync(animal);
            _animals.Add(animal);
            OnSave?.Invoke(animal);
        }

        public async Task Modify( Animal animal )
        {
            await _animalService.UpdateAnimalAsync(animal);
            int currentIndex = _animals.FindIndex(x => x.AnimalId == animal.AnimalId);

            if (currentIndex != -1)
            {
                _animals[currentIndex] = animal;
            }
            else
            {
                _animals.Add(animal);
            }
            OnUpdate?.Invoke(animal);
        }

        public async Task Remove( int id )
        {
            await _animalService.DeleteAnimalAsync(id);
            _animals.RemoveAll(a => a.AnimalId == id);
            OnDelete?.Invoke(id);
        }

        #endregion

        #region Events

        public event Action? OnLoad;
        public event Action<Animal>? OnSave;
        public event Action<Animal>? OnUpdate;
        public event Action<int>? OnDelete;

        #endregion

        #region Methods

        public async Task Load()
        {
            try
            {
                await _initializeTask.Value;
            }
            catch (Exception )
            {
                _initializeTask = new Lazy<Task>(Init);
                throw;
            }
            finally
            {
                OnLoad?.Invoke();
            }
        }


        private async Task Init()
        {
            _animals.Clear();
            IEnumerable<Animal> animals = await _animalService.GetAllAnimalsAsync();
            _animals.AddRange(animals);
        }

        #endregion

        #region Fields

        private readonly List<Animal> _animals = [];
        private readonly IAnimalService _animalService;
        private Lazy<Task> _initializeTask;

        #endregion
    }
}
