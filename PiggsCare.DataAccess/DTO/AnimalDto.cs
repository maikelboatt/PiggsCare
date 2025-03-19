namespace PiggsCare.DataAccess.DTO
{
    public class AnimalDto
    {
        public int AnimalId { get; init; }
        public required int Name { get; init; }
        public required string Breed { get; init; }
        public required DateTime BirthDate { get; init; }
        public required int CertificateNumber { get; init; }

        public required string Gender { get; init; }

        public required float BackFatIndex { get; init; }
        
    // // Navigation properties (for relationships)
    // public List<BreedingEventDto> BreedingEvents { get; set; }
    // public List<HealthRecordDto> HealthRecords { get; set; }
    // public RemovalEventDto RemovalEventDto { get; set; }
    }
}
