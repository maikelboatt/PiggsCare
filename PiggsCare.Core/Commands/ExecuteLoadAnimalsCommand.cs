using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;

namespace PiggsCare.Core.Commands
{
    public class ExecuteLoadAnimalsCommand( IAnimalService animalService ):AsyncCommandBase
    {
        private void WriteToConsole( IEnumerable<Animal?> animals )
        {
            foreach (Animal? animal in animals)
            {
                Console.WriteLine(animal);
            }
        }

        protected override async Task ExecuteAsync( object? parameter )
        {
            // Animal animal = new(102, "oestrous", DateTime.Now, 234);
            IEnumerable<Animal?> result;
            try
            {
                // await animalService.CreateAnimalAsync(animal);
                result = await animalService.GetAllAnimalsAsync();
                await animalService.UpdateAnimalAsync(new Animal
                {
                    AnimalId = 2,
                    Name = 010,
                    Breed = "Sow",
                    BirthDate = DateTime.Now,
                    CertificateNumber = 334
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            // Console.WriteLine($"You have successfully inserted animal with identification: {animal.Name} into the database.");
            WriteToConsole(result);
        }
    }
}
