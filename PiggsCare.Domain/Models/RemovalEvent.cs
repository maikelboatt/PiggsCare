namespace PiggsCare.Domain.Models
{
    public class RemovalEvent( int removalEventId, int animalId, DateOnly removalDate, string reasonForRemoval )
    {
        public int RemovalEventId { get; init; } = removalEventId;
        public int AnimalId { get; init; } = animalId;
        public DateOnly RemovalDate { get; init; } = removalDate;
        public string ReasonForRemoval { get; init; } = reasonForRemoval;

        public override string ToString()
        {
            return $"Removal Event Id: {RemovalEventId}, Animal Id: {AnimalId}, Removal Date: {RemovalDate}, Reason for Removal: {ReasonForRemoval}";
        }
    }
}
