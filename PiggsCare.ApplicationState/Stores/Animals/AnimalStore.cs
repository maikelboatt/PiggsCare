using Microsoft.Extensions.Logging;
using PiggsCare.Domain.Models;

namespace PiggsCare.ApplicationState.Stores.Animals
{
    public class AnimalStore( ILogger<AnimalStore> logger ):IAnimalStore
    {
        private readonly List<Animal> _animals = [];

        /// <summary>
        ///     Lock for thread-safe read/write operations on the animals' list.
        /// </summary>
        private readonly ReaderWriterLockSlim _lock = new();

        private readonly ILogger<AnimalStore> _logger = logger ?? throw new ArgumentNullException(nameof(logger));


        public IEnumerable<Animal> Animals
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _animals.ToList();
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }

        public void LoadAnimals( IEnumerable<Animal> animals, CancellationToken cancellationToken = default )
        {
            try
            {
                _lock.EnterWriteLock();
                try
                {
                    _animals.Clear();
                    _animals.AddRange(animals);
                }
                finally
                {
                    _lock.ExitWriteLock();
                }

                OnAnimalsLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error loading members: {ExMessage}", ex.Message);
                throw;
            }
        }

        public Animal? GetAnimalById( int animalId )
        {
            _lock.EnterReadLock();
            try
            {
                return _animals.FirstOrDefault(a => a.AnimalId == animalId);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public Animal? GetAnimalByName( int name )
        {
            _lock.EnterReadLock();
            try
            {
                return _animals.FirstOrDefault(a => a.Name == name);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public IEnumerable<Animal>? GetAnimalByBreed( string breed )
        {
            _lock.EnterReadLock();
            try
            {
                List<Animal> result = _animals.Where(a => a.Breed == breed).ToList();
                return result.Count != 0 ? result : null;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void AddAnimal( Animal animal )
        {
            _lock.EnterWriteLock();
            try
            {
                _animals.Add(animal);
                OnAnimalAdded?.Invoke(animal);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public Animal CreateAnimalWithCorrectAnimalId( int animalId, Animal animal ) => new(
            animalId,
            animal.Name,
            animal.Breed,
            animal.BirthDate,
            animal.CertificateNumber,
            animal.Gender,
            animal.BackFatIndex
        );

        public void UpdateAnimal( Animal animal )
        {
            _lock.EnterWriteLock();
            try
            {
                int index = _animals.FindIndex(a => a.AnimalId == animal.AnimalId);
                if (index == -1) return;
                _animals[index] = animal;
                OnAnimalUpdated?.Invoke(animal);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void DeleteAnimal( int animalId )
        {
            _lock.EnterWriteLock();
            try
            {
                Animal? animal = _animals.FirstOrDefault(m => m.AnimalId == animalId);
                if (animal == null) return;
                _animals.Remove(animal);
                OnAnimalDeleted?.Invoke(animal);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public event Action? OnAnimalsLoaded;
        public event Action<Animal>? OnAnimalAdded;
        public event Action<Animal>? OnAnimalUpdated;
        public event Action<Animal>? OnAnimalDeleted;
    }
}
