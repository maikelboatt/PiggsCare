namespace PiggsCare.Domain.Models
{
    public class InseminationEventWithAnimal(
        int breedingEventId,
        int animalId,
        DateOnly aiDate,
        DateOnly expectedFarrowDate,
        int? synchronizationEventId,
        string? name )
        :InseminationEvent(breedingEventId, animalId, aiDate, expectedFarrowDate, synchronizationEventId)
    {
        public string? Name { get; init; } = name;

        public override string ToString() => $"{base.ToString()},Animal Name: {Name}";
    }
}
