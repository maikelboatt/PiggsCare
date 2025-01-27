namespace PiggsCare.DataAccess.DTO
{
    public class FarrowingEventDto
    {
        public int FarrowingEventId { get; init; }
        public required int BreedingEventId { get; init; }
        public required DateTime FarrowDate { get; set; }
        public required int LitterSize { get; set; }
        public required int BornAlive { get; set; }
        public required int BornDead { get; set; }
        public required int Mummified { get; set; }

        // // Navigation properties
        // public BreedingEventDto BreedingEventDto { get; set; }
        // public List<PigletDto> Piglets { get; set; }
        // public WeaningEventDto WeaningEventDto { get; set; }
    }
}
