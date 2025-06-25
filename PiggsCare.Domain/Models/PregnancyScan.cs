namespace PiggsCare.Domain.Models
{
    public class PregnancyScan( int scanId, int breedingEventId, DateOnly scanDate, string scanResults )
    {
        public int ScanId { get; init; } = scanId;
        public int BreedingEventId { get; init; } = breedingEventId;
        public DateOnly ScanDate { get; set; } = scanDate;
        public string ScanResults { get; set; } = scanResults;

        public override string ToString() => $"Scan Id: {ScanId}, Breeding Event Id: {BreedingEventId}, ScanDate: {ScanDate}, ScanResults: {ScanResults}";

        public override bool Equals( object? obj )
        {
            if (obj is not PregnancyScan other)
                return false;

            return ScanId == other.ScanId;
        }

        /// <summary>
        ///     Returns a hash code for the current animal.
        /// </summary>
        /// <returns>A hash code for the current animal.</returns>
        public override int GetHashCode() => ScanId.GetHashCode();
    }
}
