using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.Animals
{
    /// <summary>
    ///     Defines methods for managing animal entities.
    /// </summary>
    public interface IAnimalService
    {
        /// <summary>
        ///     Creates a new animal asynchronously.
        /// </summary>
        /// <param name="animal" >The animal to create.</param>
        /// <returns>The ID of the created animal.</returns>
        Task CreateAnimalAsync( Animal animal );

        /// <summary>
        ///     Deletes an animal by its ID asynchronously.
        /// </summary>
        /// <param name="id" >The ID of the animal to delete.</param>
        Task DeleteAnimalAsync( int id );

        /// <summary>
        ///     Retrieves all animals asynchronously.
        /// </summary>
        Task GetAllAnimalsAsync();

        /// <summary>
        ///     Retrieves animals by their breed asynchronously.
        /// </summary>
        /// <param name="breed" >
        ///     The breed to filter animals by.
        /// </param>
        /// <returns>
        ///     An enumerable collection of animals matching the specified breed.
        /// </returns>
        IEnumerable<Animal>? GetAnimalByBreedAsync( string breed );

        /// <summary>
        ///     Retrieves an animal by its ID asynchronously.
        /// </summary>
        /// <param name="id" >
        ///     The ID of the animal to retrieve.
        /// </param>
        /// <returns>
        ///     The animal with the specified ID, or <c>null</c> if not found.
        /// </returns>
        Animal? GetAnimalById( int id );

        /// <summary>
        ///     Retrieves an animal by its name asynchronously.
        /// </summary>
        /// <param name="name" >
        ///     The name of the animal to retrieve.
        /// </param>
        /// <returns>
        ///     The animal with the specified name, or <c>null</c> if not found.
        /// </returns>
        Animal? GetAnimalByNameAsync( int name );

        /// <summary>
        ///     Updates an existing animal asynchronously.
        /// </summary>
        /// <param name="animal" >The animal with updated information.</param>
        Task UpdateAnimalAsync( Animal animal );
    }
}
