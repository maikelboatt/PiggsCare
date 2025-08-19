// This is a ViewModel for displaying scheduled notifications in a MvvmCross application.
// It informs the user about scheduled insemination notifications and allows them to create insemination events.
// The plan is to display a list of scheduled notifications, and when the user clicks on the OpenAnimalDetailsDialogCommand, it will send the insemination Detail to the ScheduledNotificationAnimalListingViewModel.

using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores;
using PiggsCare.ApplicationState.Stores.ScheduledNotifications;
using PiggsCare.Business.Services.Insemination;
using PiggsCare.Business.Services.ScheduledNotifications;
using PiggsCare.Core.Control;
using PiggsCare.Core.Parameter;
using PiggsCare.Domain.Models;

namespace PiggsCare.Core.ViewModels.Notifications
{
    public class ScheduleNotificationListingViewModel:MvxViewModel, IScheduleNotificationListingViewModel
    {
        private readonly IInseminationService _inseminationService;
        private readonly ILogger<ScheduleNotificationListingViewModel> _logger;
        private readonly IModalNavigationControl _modalNavigationControl;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly MvxObservableCollection<ScheduledNotification> _scheduledNotifications;
        private readonly IScheduledNotificationService _scheduledNotificationService;
        private readonly IScheduledNotificationStore _scheduledNotificationStore;

        private readonly DateOnly _today = DateOnly.FromDateTime(DateTime.Today);
        private bool _isLoading;

        public ScheduleNotificationListingViewModel( IScheduledNotificationService scheduledNotificationService, IInseminationService inseminationService,
            IScheduledNotificationStore scheduledNotificationStore,
            IModalNavigationControl modalNavigationControl, ModalNavigationStore modalNavigationStore, ILogger<ScheduleNotificationListingViewModel> logger )
        {
            _scheduledNotificationService = scheduledNotificationService;
            _inseminationService = inseminationService;
            _scheduledNotificationStore = scheduledNotificationStore;
            _modalNavigationControl = modalNavigationControl;
            _modalNavigationStore = modalNavigationStore;
            _logger = logger;

            // Initialize the Observable collection
            _scheduledNotifications = new MvxObservableCollection<ScheduledNotification>(_scheduledNotificationStore.ScheduledNotifications);

            // Initialize commands
            OpenAnimalDetailsDialogCommand = new MvxCommand<ScheduledNotification>(ExecuteOpenAnimalDetailsDialog);
            CloseDialogCommand = new MvxCommand(ExecuteCloseDialog);
            // CreateInseminationsCommand = new MvxAsyncCommand(ExecuteCreateInseminations);

            // Events Subscription
            _scheduledNotificationStore.OnScheduledNotificationsLoaded += ScheduledNotificationStoreOnOnScheduledNotificationsLoaded;

        }

        private void ScheduledNotificationStoreOnOnScheduledNotificationsLoaded()
        {
            RaisePropertyChanged(nameof(ScheduledNotifications));
        }

        private async Task ExecuteCreateInseminations( CancellationToken arg )
        {
            foreach (ScheduledNotification scheduledNotification in _scheduledNotifications)
            {
                await _inseminationService.CreateInseminationEventAsync(
                    new InseminationEvent(
                        1,
                        scheduledNotification.AnimalIds.FirstOrDefault(),
                        DateOnly.FromDateTime(DateTime.Now),
                        DateOnly.FromDateTime(DateTime.Now).AddDays(4),
                        scheduledNotification.SynchronizationId));
            }
        }

        #region ViewModel LifeCycle

        public override async Task Initialize()
        {
            await LoadScheduledNotificationsAsync();
            await base.Initialize();
        }

        public override void ViewDestroy( bool viewFinishing = true )
        {
            _scheduledNotificationStore.OnScheduledNotificationsLoaded -= ScheduledNotificationStoreOnOnScheduledNotificationsLoaded;
            base.ViewDestroy(viewFinishing);
        }

        #endregion

        #region Properties

        public IEnumerable<ScheduledNotification> ScheduledNotifications => _scheduledNotifications;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        #endregion

        #region Methods

        private async Task LoadScheduledNotificationsAsync()
        {
            IsLoading = true;
            try
            {
                _scheduledNotifications.Clear();
                await _scheduledNotificationService.GetScheduledNotificationsByExactDateAsync(_today);

                foreach (ScheduledNotification scheduledNotification in _scheduledNotificationStore.ScheduledNotifications)
                {
                    _scheduledNotifications.Add(scheduledNotification);
                }

                UpdateView();
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., log them or show a message to the user
                // For now, we just log it
                _logger.LogError(ex, "Error loading scheduled notifications");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void UpdateView()
        {
            RaisePropertyChanged(nameof(ScheduledNotifications));
        }

        private void ExecuteCloseDialog()
        {
            _modalNavigationStore.Close();
        }

        private void ExecuteOpenAnimalDetailsDialog( ScheduledNotification? scheduledNotification )
        {
            List<InseminationEvent> events = scheduledNotification.AnimalIds.Select(animalId => new InseminationEvent(
                                                                                        1,
                                                                                        animalId,
                                                                                        DateOnly.FromDateTime(DateTime.Now),
                                                                                        DateOnly.FromDateTime(DateTime.Now).AddDays(4),
                                                                                        scheduledNotification.SynchronizationId)).ToList();
            InseminationDetailAnimalList detailParam = new(events);
            _modalNavigationControl.PopUp<ScheduledNotificationAnimalListingViewModel>(detailParam);
        }

        #endregion

        #region Commands

        public IMvxCommand<ScheduledNotification> OpenAnimalDetailsDialogCommand { get; }
        public IMvxCommand<List<int>> OpenRemoveRecordDialogCommand { get; }
        public MvxCommand CloseDialogCommand { get; }

        #endregion
    }
}
