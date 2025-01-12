namespace PiggsCare.Domain.DTO
{
    public class WeaningEventDto
    {
        public int WeaningEventID { get; init; }
        public int FarrowingEventID { get; init; }
        public DateTime WeaningDate { get; set; }
        public int NumberWeaned { get; set; }
        public int MalesWeaned { get; set; }
        public int FemalesWeaned { get; set; }
        public float AverageWeaningWeight { get; set; }

        // Navigation property
        public FarrowingEventDto FarrowingEventDto { get; set; }
    }
}
