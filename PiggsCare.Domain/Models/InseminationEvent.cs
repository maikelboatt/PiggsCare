namespace PiggsCare.Domain.Models
{
    public class InseminationEvent( int breedingEventId, int animalId, DateOnly aiDate, DateOnly expectedFarrowDate, int? synchronizationEventId )
    {
        public int BreedingEventId { get; init; } = breedingEventId;
        public int AnimalId { get; init; } = animalId;
        public DateOnly AiDate { get; init; } = aiDate;
        public DateOnly ExpectedFarrowDate { get; init; } = expectedFarrowDate;

        public int? SynchronizationEventId { get; init; } = synchronizationEventId;

        public override string ToString() =>
            $"Breeding Event Id: {BreedingEventId}, Animal Id: {AnimalId}, AiDate: {AiDate}, Expected FarrowDate: {ExpectedFarrowDate}, Sync Event Id: {{SynchronizationEventId}}";

        public override bool Equals( object? obj )
        {
            if (obj is not InseminationEvent other)
                return false;

            return BreedingEventId == other.BreedingEventId;
        }

        /// <summary>
        ///     Returns a hash code for the current animal.
        /// </summary>
        /// <returns>A hash code for the current animal.</returns>
        public override int GetHashCode() => BreedingEventId.GetHashCode();
    }
}
