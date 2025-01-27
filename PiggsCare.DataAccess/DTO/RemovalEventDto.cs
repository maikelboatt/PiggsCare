namespace PiggsCare.DataAccess.DTO
{
    public class RemovalEventDto
    {
        public int RemovalEventId { get; init; }
        public required int AnimalId { get; init; }
        public required DateTime RemovalDate { get; set; }
        public required string ReasonForRemoval { get; set; }

        // // Navigation property
        // public AnimalDto AnimalDto { get; set; }
    }
}
