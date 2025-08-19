namespace PiggsCare.DataAccess.DTO
{
    public class PregnancyScanDto
    {
        public int ScanId { get; init; }
        public required int BreedingEventId { get; init; }
        public required DateTime ScanDate { get; set; }
        public required string ScanResults { get; set; }

    }
}
