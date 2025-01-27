namespace PiggsCare.DataAccess.DTO
{
    public class HealthRecordDto
    {
        public int HealthRecordId { get; init; }
        public required int AnimalId { get; init; }
        public required DateTime RecordDate { get; set; }
        public required string Diagnosis { get; set; }
        public required string Treatment { get; set; }
        public required string Outcome { get; set; }

        // // Navigation property
        // public AnimalDto AnimalDto { get; set; }
    }
}
