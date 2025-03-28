namespace PiggsCare.DataAccess.DTO
{
    public class BreedingEventDto
    {
        public int BreedingEventId { get; init; }
        public required int AnimalId { get; init; }
        public required DateTime AiDate { get; init; }
        public required DateTime ExpectedFarrowDate { get; init; } // Nullable DateTime

        public int? SynchronizationEventId { get; init; }
    }
}


