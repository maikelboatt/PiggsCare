namespace PiggsCare.Domain.DTO
{
    public class PigletDto
    {
        public int PigletID { get; init; }
        public int FarrowingEventID { get; init; }

        // Navigation property
        public FarrowingEventDto FarrowingEventDto { get; set; }
    }
}
