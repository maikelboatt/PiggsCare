using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores.ScheduledNotifications;
using PiggsCare.Business.Services.Reports;
using PiggsCare.Core.Control;
using PiggsCare.Domain.Models;

namespace PiggsCare.Core.ViewModels
{
    public class DashboardViewModel:MvxViewModel
    {
        private readonly IModalNavigationControl _modalNavigationControl;
        private readonly IReportService _reportService;
        private readonly IScheduledNotificationStore _scheduledNotificationStore;
        private IEnumerable<Animal> _animals;
        private bool _isLoading;

        /// <summary>
        ///     Stores the chart series data.
        /// </summary>
        private ISeries[] _series;

        public DashboardViewModel( IReportService reportService, IScheduledNotificationStore scheduledNotificationStore, IModalNavigationControl modalNavigationControl )
        {
            _reportService = reportService;
            _scheduledNotificationStore = scheduledNotificationStore;
            _modalNavigationControl = modalNavigationControl;

        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        /// <summary>
        ///     Gets or sets the chart series data.
        /// </summary>
        public ISeries[] Series
        {
            get => _series;
            set => SetProperty(ref _series, value);
        }

        public override async Task Initialize()
        {
            IsLoading = true;
            try
            {
                _animals = await _reportService.LoadAnimalsFromDatabase() ?? [];
                UpdateSeries();
            }
            finally
            {
                IsLoading = false;
            }
            await base.Initialize();
        }

        private void UpdateSeries()
        {
            Series = _animals.GroupBy(a => a.Gender)
                             .Select(g => new PieSeries<int>
                             {
                                 Values =
                                 [
                                     g.Count()
                                 ],
                                 Name = g.Key
                             })
                             .Cast<ISeries>()
                             .ToArray();
        }
    }
}
