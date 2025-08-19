using PiggsCare.ApplicationState.Stores.Animals;
using PiggsCare.Business.Services.Errors;
using PiggsCare.Business.Services.ScheduledNotifications;
using PiggsCare.DataAccess.Repositories.Animals;
using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.Animals
{
    /// <summary>
    ///     Service for managing animal-related operations.
    /// </summary>
    /// <param name="animalRepository" >Repository for animal data access.</param>
    /// <param name="animalStore" >Store for animal state management.</param>
    public class AnimalService(
        IAnimalRepository animalRepository,
        IAnimalStore animalStore,
        IDatabaseErrorHandlerService databaseErrorHandlerService,
        IScheduledNotificationService scheduledNotificationService ):IAnimalService
    {
        /// <summary>
        ///     Retrieves all animals asynchronously and updates the animal store.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GetAllAnimalsAsync()
        {
            IEnumerable<Animal> animals = await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                                              animalRepository.GetAllAnimalsAsync,
                                              "Retrieving all animals")
                                          ?? [];
            animalStore.LoadAnimals(animals);
        }

        /// <summary>
        ///     Retrieves an animal by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id" >The animal's unique identifier.</param>
        /// <returns>The animal if found; otherwise, null.</returns>
        public Animal? GetAnimalById( int id ) => animalStore.GetAnimalById(id);

        /// <summary>
        ///     Retrieves animals by their breed asynchronously.
        /// </summary>
        /// <param name="breed" >The breed of the animals.</param>
        /// <returns>A collection of animals of the specified breed.</returns>
        public IEnumerable<Animal>? GetAnimalByBreedAsync( string breed ) => animalStore.GetAnimalByBreed(breed);

        /// <summary>
        ///     Creates a new animal asynchronously.
        /// </summary>
        /// <param name="animal" >The animal to create.</param>
        /// <returns>The unique identifier of the created animal.</returns>
        public async Task CreateAnimalAsync( Animal animal )
        {
            int? uniqueId = await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => animalRepository.CreateAnimalAsync(animal),
                "Adding animals"
            );
            {
                Animal animalWithCorrectId = animalStore.CreateAnimalWithCorrectAnimalId(uniqueId.Value, animal);
                animalStore.AddAnimal(animalWithCorrectId);
            }
        }

        /// <summary>
        ///     Updates an existing animal asynchronously.
        /// </summary>
        /// <param name="animal" >The animal to update.</param>
        public async Task UpdateAnimalAsync( Animal animal )
        {
            await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => animalRepository.UpdateAnimalAsync(animal),
                "Updating animals"
            );
            animalStore.UpdateAnimal(animal);
        }

        /// <summary>
        ///     Deletes an animal by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id" >The animal's unique identifier.</param>
        public async Task DeleteAnimalAsync( int id )
        {
            await databaseErrorHandlerService.HandleDatabaseOperationAsync(
                () => animalRepository.DeleteAnimalAsync(id),
                "Deleting animals"
            );
            animalStore.DeleteAnimal(id);
        }

        /// <summary>
        ///     Retrieves an animal by its name asynchronously.
        /// </summary>
        /// <param name="name" >The animal's name.</param>
        /// <returns>The animal if found; otherwise, null.</returns>
        public Animal? GetAnimalByNameAsync( int name ) => animalStore.GetAnimalByName(name);
    }
}
