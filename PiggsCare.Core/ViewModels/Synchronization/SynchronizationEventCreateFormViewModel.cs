using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores;
using PiggsCare.Business.Services.Insemination;
using PiggsCare.Business.Services.Message;
using PiggsCare.Business.Services.ScheduledNotifications;
using PiggsCare.Business.Services.Synchronization;
using PiggsCare.Core.Validation;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Services;
using System.Collections;
using System.ComponentModel;
using System.Windows;

namespace PiggsCare.Core.ViewModels.Synchronization
{
    /// <summary>
    ///     ViewModel for creating a synchronization event, handling form state, validation, and commands.
    /// </summary>
    public class SynchronizationEventCreateFormViewModel:MvxViewModel<List<Animal>>, ISynchronizationEventCreateFormViewModel
    {
        /// <summary>
        ///     The number of days in the synchronization period.
        /// </summary>
        private const int SynchronizationPeriodInDays = 18;

        /// <summary>
        ///     The number of days to add to the end date to calculate the AI (Artificial Insemination) date.
        /// </summary>
        private const int DaysToAddForAi = 4;

        /// <summary>
        ///     The number of days to add for the gestation period (typically 114 days for pigs).
        /// </summary>
        private const int DaysToAddForGestation = 114;

        /// <summary>
        ///     Service for converting dates.
        /// </summary>
        private readonly IDateConverterService _dateConverterService;

        /// <summary>
        ///     Service for handling insemination events.
        /// </summary>
        private readonly IInseminationService _inseminationService;

        /// <summary>
        ///     Service for displaying messages to the user.
        /// </summary>
        private readonly IMessageService _messageService;

        /// <summary>
        ///     Store for modal navigation.
        /// </summary>
        private readonly ModalNavigationStore _modalNavigationStore;

        /// <summary>
        ///     Store for notifications.
        /// </summary>
        private readonly INotificationStore _notificationStore;

        /// <summary>
        ///     Validation logic for synchronization records.
        /// </summary>
        private readonly ISynchronizationRecordValidation _recordValidation = new SynchronizationRecordValidation();

        private readonly IScheduledNotificationService _scheduledNotificationService;

        /// <summary>
        ///     Service for handling synchronization events.
        /// </summary>
        private readonly ISynchronizationService _synchronizationService;

        // The batch number for the synchronization event.
        private string _batchNumber = string.Empty;

        // Comments for the synchronization event.
        private string _comments = string.Empty;

        // The end date of the synchronization event.
        private DateOnly _endDate;

        /// The list of selected animals for the event.
        private List<Animal> _selectedAnimals = [];

        /// The start date of the synchronization event.
        private DateTime _startDate = DateTime.Now;

        /// The ID of the created synchronization event.
        private int _synchronizationId;

        /// The synchronization protocol.
        private string _synchronizationProtocol = string.Empty;

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="SynchronizationEventCreateFormViewModel" /> class.
        /// </summary>
        /// <param name="synchronizationService" >Service for synchronization events.</param>
        /// <param name="scheduledNotificationService" >Service for scheduled notifications</param>
        /// <param name="dateConverterService" >Service for date conversion.</param>
        /// <param name="inseminationService" >Service for insemination events.</param>
        /// <param name="messageService" >Service for user messages.</param>
        /// <param name="modalNavigationStore" >Store for modal navigation.</param>
        /// <param name="notificationStore" >Store for notifications.</param>
        public SynchronizationEventCreateFormViewModel( ISynchronizationService synchronizationService, IScheduledNotificationService scheduledNotificationService,
            IDateConverterService dateConverterService, IInseminationService inseminationService, IMessageService messageService, ModalNavigationStore modalNavigationStore,
            INotificationStore notificationStore )
        {
            _synchronizationService = synchronizationService;
            _scheduledNotificationService = scheduledNotificationService;
            _modalNavigationStore = modalNavigationStore;
            _dateConverterService = dateConverterService;
            _inseminationService = inseminationService;
            _messageService = messageService;
            _notificationStore = notificationStore;

            _recordValidation.ErrorsChanged += RecordValidationOnErrorsChanged;

            // Initialize commands once so that RaiseCanExecuteChanged works as expected
            SubmitRecordCommand = new MvxAsyncCommand(ExecuteSubmitRecord, CanSubmitRecord);
            CancelRecordCommand = new MvxCommand(ExecuteCancelCommand);
        }

        #endregion

        #region ViewModel Life-Cycle

        /// <summary>
        ///     Prepares the ViewModel with a list of animals.
        /// </summary>
        /// <param name="parameter" >The list of animals.</param>
        public override void Prepare( List<Animal> parameter )
        {
            _selectedAnimals = parameter;
        }

        /// <summary>
        ///     Cleans up resources when the view is being destroyed.
        /// </summary>
        /// <param name="viewFinishing" >Indicates if the view is finishing.</param>
        public override void ViewDestroy( bool viewFinishing = true )
        {
            base.ViewDestroy(viewFinishing);
            _recordValidation.ErrorsChanged -= RecordValidationOnErrorsChanged;
        }

        #endregion

        #region INotifyDataErrorInfo Implementation

        /// <summary>
        ///     Gets validation errors for a specific property.
        /// </summary>
        /// <param name="propertyName" >The name of the property.</param>
        /// <returns>An enumerable of validation errors.</returns>
        public IEnumerable GetErrors( string? propertyName ) => _recordValidation.GetErrors(propertyName);

        /// <summary>
        ///     Gets a value indicating whether there are validation errors.
        /// </summary>
        public bool HasErrors => _recordValidation.HasErrors;

        /// <summary>
        ///     Occurs when the validation errors have changed.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        /// <summary>
        ///     Handles changes in validation errors.
        /// </summary>
        /// <param name="sender" >The event sender.</param>
        /// <param name="e" >The event arguments.</param>
        private void RecordValidationOnErrorsChanged( object? sender, DataErrorsChangedEventArgs e )
        {
            ErrorsChanged?.Invoke(this, e);
            RaisePropertyChanged(nameof(HasErrors));
            SubmitRecordCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the AI date, calculated as EndDate plus 4 days.
        /// </summary>
        private DateOnly AiDate => EndDate.AddDays(DaysToAddForAi);

        /// <summary>
        ///     Gets or sets the start date of the synchronization event.
        /// </summary>
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (value.Equals(_startDate)) return;
                _startDate = value;
                _recordValidation.ValidateProp(_dateConverterService.GetDateOnly(_startDate));
                RaisePropertyChanged();
                CalculateEndDate();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the end date of the synchronization event.
        /// </summary>
        public DateOnly EndDate
        {
            get => _endDate;
            set
            {
                if (value.Equals(_endDate)) return;
                _endDate = value;
                _recordValidation.ValidateProp(_endDate);
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the synchronization protocol.
        /// </summary>
        public string SynchronizationProtocol
        {
            get => _synchronizationProtocol;
            set
            {
                if (value.Equals(_synchronizationProtocol)) return;
                _synchronizationProtocol = value;
                _recordValidation.ValidateProp(value);
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the batch number.
        /// </summary>
        public string BatchNumber
        {
            get => _batchNumber;
            set
            {
                if (value.Equals(_batchNumber)) return;
                _batchNumber = value;
                _recordValidation.ValidateProp(_batchNumber);
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the comments.
        /// </summary>
        public string Comments
        {
            get => _comments;
            set
            {
                if (value.Equals(_comments)) return;
                _comments = value;
                RaisePropertyChanged();
                _recordValidation.ValidateProp(_comments);
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Commands

        /// <summary>
        ///     Command to submit the synchronization event record.
        /// </summary>
        public IMvxAsyncCommand SubmitRecordCommand { get; }

        /// <summary>
        ///     Determines whether the submit record command can execute.
        /// </summary>
        /// <returns>True if the command can execute; otherwise, false.</returns>
        private bool CanSubmitRecord()
        {
            bool noFieldEmpty = !string.IsNullOrWhiteSpace(BatchNumber) && !string.IsNullOrWhiteSpace(SynchronizationProtocol) && !string.IsNullOrWhiteSpace(Comments) &&
                                !StartDate.Equals(default) && !EndDate.Equals(default);
            return noFieldEmpty && !HasErrors;
        }

        /// <summary>
        ///     Command to cancel the synchronization event record creation.
        /// </summary>
        public IMvxCommand CancelRecordCommand { get; }

        #endregion

        #region Methods

        /// <summary>
        ///     Executes the submit record command.
        /// </summary>
        private async Task ExecuteSubmitRecord()
        {
            SynchronizationEvent record = GetSynchronizationEventFromFields();
            _synchronizationId = await _synchronizationService.CreateSynchronizationEventAsync(record);
            await ConfirmInseminationReminder();
            _modalNavigationStore.Close();
        }

        /// <summary>
        ///     Confirms if the user wants to create reminders for this synchronization event.
        /// </summary>
        private async Task ConfirmInseminationReminder()
        {
            MessageBoxResult result = _messageService.Show(
                "Do you want to create a reminder for these Insemination records?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                await CreateReminderForInsemination();
        }


        private async Task CreateReminderForInsemination()
        {
            List<int> ids = GetIdsOfSelectedAnimals();

            const string message = "Reminder: Artificial Insemination for animal(s) has to be done.";

            ScheduledNotification notification = new(1, message, AiDate, ids, _synchronizationId);

            await _scheduledNotificationService.CreateScheduledNotificationAsync(notification);
        }

        private List<int> GetIdsOfSelectedAnimals()
        {
            List<int> ids = [];
            ids.AddRange(_selectedAnimals.Select(animal => animal.AnimalId));
            return ids;
        }

        /// <summary>
        ///     Executes the cancel command.
        /// </summary>
        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        /// <summary>
        ///     Gets a synchronization event from the form fields.
        /// </summary>
        /// <returns>A new synchronization event.</returns>
        private SynchronizationEvent GetSynchronizationEventFromFields() => new(
            1,
            _dateConverterService.GetDateOnly(_startDate),
            _endDate,
            _batchNumber,
            _synchronizationProtocol,
            _comments);

        /// <summary>
        ///     Gets an insemination event from the form fields for a specific animal.
        /// </summary>
        /// <param name="animalId" >The ID of the animal.</param>
        /// <returns>A new insemination event.</returns>
        private InseminationEvent GetBreedingEventFromFields( int animalId ) => new(1, animalId, AiDate, AiDate.AddDays(DaysToAddForGestation), _synchronizationId);

        /// <summary>
        ///     Calculates the end date of the synchronization period based on the start date.
        /// </summary>
        private void CalculateEndDate()
        {
            DateTime result = StartDate.AddDays(SynchronizationPeriodInDays);
            EndDate = _dateConverterService.GetDateOnly(result);
        }

        #endregion
    }
}
