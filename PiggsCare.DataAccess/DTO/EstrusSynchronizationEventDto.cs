namespace PiggsCare.DataAccess.DTO
{
    public class EstrusSynchronizationEventDto
    {
        public int EstrusSynchronizationEventId { get; init; }
        public required DateTime StartDate { get; init; }
        public required DateTime EndDate { get; set; }
        public required string BatchNumber { get; set; }

        // public List<int> AnimalIds { get; init; } = [];
    }
}
