namespace PiggsCare.Domain.DTO
{
    public class EstrusSynchronizationEventDto
    {
        public int EstrusSynchronizationEventId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; set; }
        public string BatchNumber { get; set; }

        public List<int> AnimalIds { get; init; } = [];
    }
}
