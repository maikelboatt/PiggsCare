using PiggsCare.ApplicationState.Stores.Animals;
using PiggsCare.Business.Services.Animals;
using PiggsCare.Business.Services.ScheduledNotifications;
using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.Reports
{
    public class ReportService( IAnimalService animalService, IAnimalStore animalStore, IScheduledNotificationService scheduledNotificationService ):IReportService
    {
        public async Task<IEnumerable<Animal>> LoadAnimalsFromDatabase()
        {
            await animalService.GetAllAnimalsAsync();
            return animalStore.Animals;
        }

        public async Task LoadScheduledNotificationsFromDatabase()
        {
            await scheduledNotificationService.GetScheduledNotificationsByExactDateAsync(DateOnly.FromDateTime(DateTime.Now));
        }
    }
}
