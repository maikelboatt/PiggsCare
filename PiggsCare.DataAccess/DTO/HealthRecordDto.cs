namespace PiggsCare.DataAccess.DTO
{
    public class HealthRecordDto
    {
        public int HealthRecordId { get; init; }
        public required int AnimalId { get; init; }
        public required DateTime RecordDate { get; init; }
        public required string Diagnosis { get; init; }
        public required string Treatment { get; init; }
        public required string Outcome { get; init; }

        // // Navigation property
        // public AnimalDto AnimalDto { get; set; }
    }
}
