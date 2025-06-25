namespace PiggsCare.Domain.Models
{
    public class RemovalEvent( int removalEventId, int animalId, DateOnly removalDate, string reasonForRemoval )
    {
        public int RemovalEventId { get; init; } = removalEventId;
        public int AnimalId { get; init; } = animalId;
        public DateOnly RemovalDate { get; init; } = removalDate;
        public string ReasonForRemoval { get; init; } = reasonForRemoval;

        public override string ToString() => $"Removal Event Id: {RemovalEventId}, Animal Id: {AnimalId}, Removal Date: {RemovalDate}, Reason for Removal: {ReasonForRemoval}";

        public override bool Equals( object? obj )
        {
            if (obj is not RemovalEvent other)
                return false;

            return RemovalEventId == other.RemovalEventId;
        }

        /// <summary>
        ///     Returns a hash code for the current animal.
        /// </summary>
        /// <returns>A hash code for the current animal.</returns>
        public override int GetHashCode() => RemovalEventId.GetHashCode();
    }
}
