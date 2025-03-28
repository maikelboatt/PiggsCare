namespace PiggsCare.Domain.Models
{
    public class BreedingEventWithAnimal(
        int breedingEventId,
        int animalId,
        DateOnly aiDate,
        DateOnly expectedFarrowDate,
        int? synchronizationEventId,
        string name )
        :BreedingEvent(breedingEventId, animalId, aiDate, expectedFarrowDate, synchronizationEventId)
    {
        public string Name { get; init; } = name;

        public override string ToString()
        {
            return $"{base.ToString()},Animal Name: {Name}";
        }
    }
}
