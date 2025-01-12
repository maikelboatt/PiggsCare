namespace PiggsCare.Domain.DTO
{
    public class HealthRecordDto
    {
        public int HealthRecordId { get; init; }
        public int AnimalId { get; init; }
        public DateTime RecordDate { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string Outcome { get; set; }

        // Navigation property
        public AnimalDto AnimalDto { get; set; }
    }
}
