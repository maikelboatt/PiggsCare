using PiggsCare.Domain.Models;

namespace PiggsCare.Domain.DTO
{
    public class AnimalDto
    {
        public int AnimalId { get; init; }
        public int AnimalIdentification { get; init; }
        public string Breed { get; set; }
        public DateTime BirthDate { get; init; }
        public string CertificateNumber { get; set; }
        public Gender Gender { get; init; }
        public float? BackFatIndex { get; set; }

        // Navigation properties (for relationships)
        public List<BreedingEventDto> BreedingEvents { get; set; }
        public List<HealthRecordDto> HealthRecords { get; set; }
        public RemovalEventDto RemovalEventDto { get; set; }
    }
}
