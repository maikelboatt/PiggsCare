namespace PiggsCare.Domain.DTO
{
    public class PregnancyScanDto
    {
        public int ScanID { get; init; }
        public int BreedingEventID { get; init; }
        public DateTime ScanDate { get; set; }
        public string ScanResults { get; set; }

        // Navigation property
        public BreedingEventDto BreedingEventDto { get; set; }
    }
}
