namespace PiggsCare.Domain.Models
{
    public class HealthRecord(
        int healthRecordId,
        int animalId,
        DateOnly recordDate,
        string diagnosis,
        string treatment,
        string outcome )
    {
        public int HealthRecordId { get; init; } = healthRecordId;
        public int AnimalId { get; init; } = animalId;
        public DateOnly RecordDate { get; init; } = recordDate;
        public string Diagnosis { get; init; } = diagnosis;
        public string Treatment { get; init; } = treatment;
        public string Outcome { get; init; } = outcome;

        public override string ToString() =>
            $"Health Record Key: {HealthRecordId}, Animal Identifier: {AnimalId}, Record Date: {RecordDate}, Diagnosis: {Diagnosis}, Treatment: {Treatment}, Outcome: {Outcome}";

        public override bool Equals( object? obj )
        {
            if (obj is not HealthRecord other)
                return false;

            return HealthRecordId == other.HealthRecordId;
        }

        /// <summary>
        ///     Returns a hash code for the current animal.
        /// </summary>
        /// <returns>A hash code for the current animal.</returns>
        public override int GetHashCode() => HealthRecordId.GetHashCode();
    }
}
