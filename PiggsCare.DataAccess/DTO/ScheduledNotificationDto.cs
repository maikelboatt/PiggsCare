namespace PiggsCare.DataAccess.DTO
{
    public class ScheduledNotificationDto
    {
        public int Id { get; init; }
        public required string Message { get; init; }
        public required DateTime ScheduledDate { get; init; }

        public int SynchronizationId { get; init; }
    }
}
