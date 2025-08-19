using MvvmCross.ViewModels;
using PiggsCare.Core.Factory;
using PiggsCare.Core.ViewModels.Notifications;

namespace PiggsCare.Core.ViewModels
{
    public class ReminderViewModel( IViewModelFactory viewModelFactory ):MvxViewModel, IReminderViewModel
    {
        public ScheduleNotificationListingViewModel? ScheduleNotificationListingViewModel => CreateViewModel();

        private ScheduleNotificationListingViewModel? CreateViewModel()
        {
            ScheduleNotificationListingViewModel? viewmodel = viewModelFactory.CreateViewModel<ScheduleNotificationListingViewModel>();
            viewmodel?.Initialize();
            return viewmodel;
        }
    }
}
