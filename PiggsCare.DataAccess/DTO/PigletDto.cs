namespace PiggsCare.DataAccess.DTO
{
    public class PigletDto
    {
        public int PigletId { get; init; }
        public required int FarrowingEventId { get; init; }

        // Navigation property
        // public FarrowingEventDto FarrowingEventDto { get; set; }
    }
}
