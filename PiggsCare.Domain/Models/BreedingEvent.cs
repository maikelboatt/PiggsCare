namespace PiggsCare.Domain.Models
{
    public class BreedingEvent( int breedingEventId, int animalId, DateTime aiDate, DateTime? expectedFarrowDate )
    {
        public int BreedingEventId { get; init; } = breedingEventId;
        public int AnimalId { get; init; } = animalId;
        public DateTime AiDate { get; init; } = aiDate;
        public DateTime? ExpectedFarrowDate { get; init; } = expectedFarrowDate; // Nullable DateTime
    }
}
