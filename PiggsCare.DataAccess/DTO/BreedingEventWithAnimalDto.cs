namespace PiggsCare.DataAccess.DTO
{
    public class BreedingEventWithAnimalDto
    {
        public int BreedingEventId { get; init; }
        public required int AnimalId { get; init; }
        public required DateTime AiDate { get; init; }
        public required DateTime ExpectedFarrowDate { get; init; } // Nullable DateTime
        public required string Name { get; init; }

        public int? SynchronizationEventId { get; init; }
    }
}
