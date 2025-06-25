namespace PiggsCare.Domain.Models
{
    public class FarrowEvent(
        int farrowingEventId,
        int breedingEventId,
        DateOnly farrowDate,
        int litterSize,
        int bornAlive,
        int bornDead,
        int mummified )
    {
        public int FarrowingEventId { get; init; } = farrowingEventId;
        public int BreedingEventId { get; init; } = breedingEventId;
        public DateOnly FarrowDate { get; set; } = farrowDate;
        public int LitterSize { get; set; } = litterSize;
        public int BornAlive { get; set; } = bornAlive;
        public int BordDead { get; set; } = bornDead;
        public int Mummified { get; set; } = mummified;


        public override string ToString() =>
            $"Farrowing Event Id: {FarrowingEventId}, Breeding Event Id: {BreedingEventId}, Farrow Date: {FarrowDate}, Litter Size: {LitterSize}, Born Alive: {BornAlive}, Born Dead: {BordDead}, Mummified : {Mummified}";

        public override bool Equals( object? obj )
        {
            if (obj is not FarrowEvent other)
                return false;

            return FarrowingEventId == other.FarrowingEventId;
        }

        /// <summary>
        ///     Returns a hash code for the current animal.
        /// </summary>
        /// <returns>A hash code for the current animal.</returns>
        public override int GetHashCode() => FarrowingEventId.GetHashCode();
    }
}
