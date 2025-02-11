namespace PiggsCare.Domain.Models
{
    public class HealthRecord(
        int healthRecordId,
        int animalId,
        DateTime recordDate,
        string diagnosis,
        string treatment,
        string outcome )
    {
        public int HealthRecordId { get; init; } = healthRecordId;
        public int AnimalId { get; init; } = animalId;
        public DateTime RecordDate { get; set; } = recordDate;
        public string Diagnosis { get; set; } = diagnosis;
        public string Treatment { get; set; } = treatment;
        public string Outcome { get; set; } = outcome;
    }
}
