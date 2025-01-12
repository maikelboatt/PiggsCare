namespace PiggsCare.Domain.DTO
{
    public class RemovalEventDto
    {
        public int RemovalEventID { get; init; }
        public int AnimalID { get; init; }
        public DateTime RemovalDate { get; set; }
        public string ReasonForRemoval { get; set; }

        // Navigation property
        public AnimalDto AnimalDto { get; set; }
    }
}
