using PiggsCare.Domain.Models;

namespace PiggsCare.Domain.DTO
{
    public class AnimalDto
    {
        public required int AnimalId { get; init; }
        public required int AnimalIdentification { get; init; }
        public required string Breed { get; set; }
        public required DateTime BirthDate { get; init; }
        public required string CertificateNumber { get; set; }
        public required Gender Gender { get; init; }
        public required float? BackFatIndex { get; set; }

        // Navigation properties (for relationships)
        public List<BreedingEventDto> BreedingEvents { get; set; }
        public List<HealthRecordDto> HealthRecords { get; set; }
        public RemovalEventDto RemovalEventDto { get; set; }
    }
}
