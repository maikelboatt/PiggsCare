using PiggsCare.Domain.Models;

namespace PiggsCare.Domain.Services
{
    public interface IAnimalService
    {
        /// <summary>
        ///     Method to Get all Animals in the database
        /// </summary>
        /// <returns>All animals in the database</returns>
        Task<IEnumerable<Animal>> GetAllAnimalsAsync();

        /// <summary>
        ///     Method that returns an animal that has a unique id
        /// </summary>
        /// <param name="id" >Unique identification of animal </param>
        /// <returns>Returns animal with said id otherwise null</returns>
        Task<Animal?> GetAnimalByIdAsync( int id );

        /// <summary>
        ///     Creates a new animal record
        /// </summary>
        /// <param name="animal" > Details of the animal</param>
        /// <returns>the id of created animal</returns>
        Task<int> CreateAnimalAsync( Animal animal );

        /// <summary>
        ///     Updates an existing record
        /// </summary>
        /// <param name="animal" >Change details</param>
        Task UpdateAnimalAsync( Animal animal );

        /// <summary>
        ///     Deletes an animal from the database
        /// </summary>
        /// <param name="id" >Unique identification of animal</param>
        Task DeleteAnimalAsync( int id );

        /// <summary>
        ///     Gets animal by name
        /// </summary>
        /// <param name="name" >Name of pig</param>
        /// <returns></returns>
        Task<Animal?> GetAnimalByNameAsync( int name );

        /// <summary>
        ///     Gets animal by breed
        /// </summary>
        /// <param name="breed" >Breed of animal</param>
        /// <returns></returns>
        Task<IEnumerable<Animal>> GetAnimalByBreedAsync( string breed );
    }
}
