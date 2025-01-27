namespace PiggsCare.DataAccess.DTO
{
    public class WeaningEventDto
    {
        public int WeaningEventId { get; init; }
        public required int FarrowingEventId { get; init; }
        public required DateTime WeaningDate { get; set; }
        public required int NumberWeaned { get; set; }
        public required int MalesWeaned { get; set; }
        public required int FemalesWeaned { get; set; }
        public required float AverageWeaningWeight { get; set; }

        // Navigation property
        public FarrowingEventDto FarrowingEventDto { get; set; }
    }
}
