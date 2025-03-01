namespace PiggsCare.Domain.Models
{
    public class BreedingEvent( int breedingEventId, int animalId, DateOnly aiDate, DateOnly expectedFarrowDate )
    {
        public int BreedingEventId { get; init; } = breedingEventId;
        public int AnimalId { get; init; } = animalId;
        public DateOnly AiDate { get; init; } = aiDate;
        public DateOnly ExpectedFarrowDate { get; init; } = expectedFarrowDate; // Nullable DateTime

        public override string ToString()
        {
            return $"Breeding Event Id: {BreedingEventId}, Animal Id: {AnimalId}, AiDate: {AiDate}, Expected FarrowDate: {ExpectedFarrowDate}";
        }
    }
}
