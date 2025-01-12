namespace PiggsCare.Domain.DTO
{
    public class BreedingEventDto
    {
        public int BreedingEventID { get; init; }
        public int AnimalID { get; init; }
        public DateTime AIDate { get; set; }
        public DateTime? ExpectedFarrowDate { get; set; } // Nullable DateTime

        // Navigation properties
        public AnimalDto AnimalDto { get; set; }
        public PregnancyScanDto PregnancyScanDto { get; set; }
        public FarrowingEventDto FarrowingEventDto { get; set; }
    }
}


//tonyboateng90@gmail.com
// taobynot
