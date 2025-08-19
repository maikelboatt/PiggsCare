namespace PiggsCare.Domain.Models
{
    public class ScheduledNotification( int notificationDtoId, string notificationDtoMessage, DateOnly getDateOnly, List<int> toList, int synchronizationId )
    {
        public int Id { get; set; } = notificationDtoId;
        public string Message { get; set; } = notificationDtoMessage;
        public DateOnly ScheduledDate { get; set; } = getDateOnly;

        public List<int> AnimalIds { get; set; } = toList;

        public int SynchronizationId { get; set; } = synchronizationId;
    }
}

// The ScheduledNotification class contains AnimalsIds because there is a probability the user would want to inseminate multiple animals at once.
