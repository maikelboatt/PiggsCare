using PiggsCare.Domain.Models;

namespace PiggsCare.ApplicationState.Stores.Animals
{
    /// <summary>
    ///     Interface for managing animal data and notifying about animal-related events.
    /// </summary>
    public interface IAnimalStore
    {
        /// <summary>
        ///     Gets the collection of animals.
        /// </summary>
        IEnumerable<Animal> Animals { get; }

        /// <summary>
        ///     Loads the provided collection of animals into the store.
        /// </summary>
        /// <param name="animals" >The animals to load.</param>
        /// <param name="cancellationToken" >Token to cancel the operation.</param>
        void LoadAnimals( IEnumerable<Animal> animals, CancellationToken cancellationToken = default );

        /// <summary>
        ///     Retrieves an animal by its unique identifier.
        /// </summary>
        /// <param name="animalId" >The animal's unique identifier.</param>
        /// <returns>The animal if found; otherwise, null.</returns>
        Animal? GetAnimalById( int animalId );

        /// <summary>
        ///     Retrieves an animal by its name.
        /// </summary>
        /// <param name="name" >The name of the animal.</param>
        /// <returns>The animal if found; otherwise, null.</returns>
        Animal? GetAnimalByName( int name );

        /// <summary>
        ///     Retrieves an animal by its breed.
        /// </summary>
        /// <param name="breed" >The breed of the animal.</param>
        /// <returns>The animal if found; otherwise, null.</returns>
        IEnumerable<Animal>? GetAnimalByBreed( string breed );

        /// <summary>
        ///     Adds a new animal to the store.
        /// </summary>
        /// <param name="animal" >The animal to add.</param>
        void AddAnimal( Animal animal );

        /// <summary>
        ///     Creates a new <see cref="Animal" /> instance with the specified animal ID,
        ///     copying the remaining properties from the provided animal.
        /// </summary>
        /// <param name="animalId" >The animal ID to assign.</param>
        /// <param name="animal" >The source animal whose properties will be copied.</param>
        /// <returns>A new <see cref="Animal" /> with the specified ID and copied properties.</returns>
        Animal CreateAnimalWithCorrectAnimalId( int animalId, Animal animal );

        /// <summary>
        ///     Updates an existing animal in the store.
        /// </summary>
        /// <param name="animal" >The animal with updated information.</param>
        void UpdateAnimal( Animal animal );

        /// <summary>
        ///     Deletes an animal from the store by its identifier.
        /// </summary>
        /// <param name="id" >The identifier of the animal to delete.</param>
        void DeleteAnimal( int id );

        /// <summary>
        ///     Event triggered when animals are loaded into the store.
        /// </summary>
        event Action OnAnimalsLoaded;

        /// <summary>
        ///     Event triggered when a new animal is added.
        /// </summary>
        event Action<Animal> OnAnimalAdded;

        /// <summary>
        ///     Event triggered when an animal is updated.
        /// </summary>
        event Action<Animal> OnAnimalUpdated;

        /// <summary>
        ///     Event triggered when an animal is deleted.
        /// </summary>
        event Action<Animal> OnAnimalDeleted;
    }
}
