namespace PiggsCare.DataAccess.DTO
{
    public class EstrusSynchronizationEventDto
    {
        public int SynchronizationEventId { get; init; }
        public required DateTime StartDate { get; init; }
        public required DateTime EndDate { get; set; }
        public required string BatchNumber { get; set; }
        public required string SynchronizationProtocol { get; set; }
        public required string Comments { get; set; }


        // Hybrid approach: link to associated insemination events
        // public List<InseminationEventDto>? BreedingEvents { get; init; }
    }
    // public List<int> AnimalIds { get; init; } = [];
}
