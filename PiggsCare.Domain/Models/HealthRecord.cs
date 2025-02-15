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
        public DateTime RecordDate { get; init; } = recordDate;
        public string Diagnosis { get; init; } = diagnosis;
        public string Treatment { get; init; } = treatment;
        public string Outcome { get; init; } = outcome;

        public override string ToString()
        {
            return $"Animal Identifier: {AnimalId}, Record Date: {RecordDate}, Diagnosis: {Diagnosis}, Treatment: {Treatment}, Outcome: {Outcome}";
        }
    }
}
