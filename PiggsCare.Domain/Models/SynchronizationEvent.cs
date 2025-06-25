namespace PiggsCare.Domain.Models
{
    public class SynchronizationEvent(
        int SynchronizationEventId,
        DateOnly startDate,
        DateOnly endDate,
        string batchNumber,
        string synchronizationProtocol,
        string comments )
    {
        public int SynchronizationEventId { get; init; } = SynchronizationEventId;
        public DateOnly StartDate { get; init; } = startDate;
        public DateOnly EndDate { get; set; } = endDate;
        public string BatchNumber { get; set; } = batchNumber;
        public string SynchronizationProtocol { get; set; } = synchronizationProtocol;
        public string Comments { get; set; } = comments;

        public override string ToString() =>
            $"Synchronization Event Id: {SynchronizationEventId}, Start Date: {StartDate}, End Date: {EndDate}, Batch Number: {BatchNumber}, Synchronization Protocol: {SynchronizationProtocol}, Comments: {Comments}";

        public override bool Equals( object? obj )
        {
            if (obj is not SynchronizationEvent other)
                return false;

            return SynchronizationEventId == other.SynchronizationEventId;
        }

        /// <summary>
        ///     Returns a hash code for the current animal.
        /// </summary>
        /// <returns>A hash code for the current animal.</returns>
        public override int GetHashCode() => SynchronizationEventId.GetHashCode();
    }
}
