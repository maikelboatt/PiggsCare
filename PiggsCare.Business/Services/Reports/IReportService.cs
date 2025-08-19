using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.Reports
{
    public interface IReportService
    {
        Task<IEnumerable<Animal>> LoadAnimalsFromDatabase();

        Task LoadScheduledNotificationsFromDatabase();
    }
}
