namespace PiggsCare.Domain.DTO
{
    public class FarrowingEventDto
    {
        public int FarrowingEventID { get; init; }
        public int BreedingEventID { get; init; }
        public DateTime FarrowDate { get; set; }
        public int LitterSize { get; set; }
        public int BornAlive { get; set; }
        public int BornDead { get; set; }
        public int Mummified { get; set; }

        // Navigation properties
        public BreedingEventDto BreedingEventDto { get; set; }
        public List<PigletDto> Piglets { get; set; }
        public WeaningEventDto WeaningEventDto { get; set; }
    }
}
