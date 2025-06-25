namespace PiggsCare.Domain.Models
{
    public class WeaningEvent(
        int weaningEventId,
        int farrowingEventId,
        DateOnly weaningDate,
        int numberWeaned,
        int malesWeaned,
        int femalesWeaned,
        float averageWeaningWeight )
    {
        public int WeaningEventId { get; init; } = weaningEventId;
        public int FarrowingEventId { get; init; } = farrowingEventId;
        public DateOnly WeaningDate { get; set; } = weaningDate;
        public int NumberWeaned { get; set; } = numberWeaned;
        public int MalesWeaned { get; set; } = malesWeaned;
        public int FemalesWeaned { get; set; } = femalesWeaned;
        public float AverageWeaningWeight { get; set; } = averageWeaningWeight;

        public override string ToString() =>
            $"Weaning Event Id: {WeaningEventId}, Farrowing Event Id: {FarrowingEventId}, Weaning Date: {WeaningDate}, Numbers Weaned: {NumberWeaned}, Males Weaned: {MalesWeaned}, Females Weaned: {FemalesWeaned}, Average Weaning Weight: {AverageWeaningWeight}";

        public override bool Equals( object? obj )
        {
            if (obj is not WeaningEvent other)
                return false;

            return WeaningEventId == other.WeaningEventId;
        }

        /// <summary>
        ///     Returns a hash code for the current animal.
        /// </summary>
        /// <returns>A hash code for the current animal.</returns>
        public override int GetHashCode() => WeaningEventId.GetHashCode();
    }
}
