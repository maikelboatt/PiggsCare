namespace PiggsCare.Domain.Models
{
    public class PregnancyScan( int scanId, int breedingEventId, DateOnly scanDate, string scanResults )
    {
        public int ScanId { get; init; } = scanId;
        public int BreedingEventId { get; init; } = breedingEventId;
        public DateOnly ScanDate { get; set; } = scanDate;
        public string ScanResults { get; set; } = scanResults;

        public override string ToString()
        {
            return $"Scan Id: {ScanId}, Breeding Event Id: {BreedingEventId}, ScanDate: {ScanDate}, ScanResults: {ScanResults}";
        }
    }
}
