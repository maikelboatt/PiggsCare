using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Core.Validation;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;
using System.Collections;
using System.ComponentModel;
using System.Windows;

namespace PiggsCare.Core.ViewModels.Synchronization
{
    /// <summary>
    ///     ViewModel for creating synchronization events for a list of animals.
    /// </summary>
    public class SynchronizationEventCreateFormViewModel:MvxViewModel<List<Animal>>, ISynchronizationEventCreateFormViewModel
    {
        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="SynchronizationEventCreateFormViewModel" /> class.
        /// </summary>
        /// <param name="synchronizationEventStore" >The store for synchronization events.</param>
        /// <param name="modalNavigationStore" >The store for modal navigation.</param>
        /// <param name="recordValidation" >The validation service for synchronization records.</param>
        /// <param name="dateConverterService" >The service for date conversion.</param>
        /// <param name="breedingEventStore" >The store for breeding events.</param>
        /// <param name="messageService" >The service for displaying messages.</param>
        /// <param name="notificationStore" >The store for notifications.</param>
        public SynchronizationEventCreateFormViewModel( ISynchronizationEventStore synchronizationEventStore, ModalNavigationStore modalNavigationStore,
            ISynchronizationRecordValidation recordValidation, IDateConverterService dateConverterService, IBreedingEventStore breedingEventStore, IMessageService messageService,
            INotificationStore notificationStore )
        {
            _synchronizationEventStore = synchronizationEventStore;
            _modalNavigationStore = modalNavigationStore;
            _recordValidation = recordValidation;
            _dateConverterService = dateConverterService;
            _breedingEventStore = breedingEventStore;
            _messageService = messageService;
            _notificationStore = notificationStore;
            _recordValidation.Errors.Clear();

            recordValidation.ErrorsChanged += RecordValidationOnErrorsChanged;

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
        public IEnumerable GetErrors( string? propertyName )
        {
            return _recordValidation.GetErrors(propertyName);
        }

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

        #region Fields

        private DateTime _startDate;
        private DateOnly _endDate;
        private string _batchNumber = string.Empty;
        private string _synchronizationProtocol = string.Empty;
        private string _comments = string.Empty;
        private List<Animal> _selectedAnimals = [];
        private DateOnly AiDate => EndDate.AddDays(4);
        private int _synchronizationId;
        private readonly ISynchronizationEventStore _synchronizationEventStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly ISynchronizationRecordValidation _recordValidation;
        private readonly IDateConverterService _dateConverterService;
        private readonly IBreedingEventStore _breedingEventStore;
        private readonly IMessageService _messageService;
        private readonly INotificationStore _notificationStore;

        private const int SynchronizationPeriodInDays = 18;

        #endregion

        #region Properties

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
            _synchronizationId = await _synchronizationEventStore.CreateAsync(record);
            ConfirmInsemination();
            _modalNavigationStore.Close();
        }

        /// <summary>
        ///     Confirms if the user wants to create insemination events for the synchronized animals.
        /// </summary>
        private void ConfirmInsemination()
        {
            MessageBoxResult result = _messageService.Show("Do you want to create an Insemination Event for these records?",
                                                           "Confirmation",
                                                           MessageBoxButton.YesNo,
                                                           MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                CreateInseminationRecordForSynchronizedAnimals();
        }

        /// <summary>
        ///     Creates insemination records for all selected animals.
        /// </summary>
        private void CreateInseminationRecordForSynchronizedAnimals()
        {
            // Create insemination events for all selected animals
            foreach (BreedingEvent record in _selectedAnimals.Select(animal => GetBreedingEventFromFields(animal.AnimalId)))
            {
                _breedingEventStore.Create(record);
            }

            int length = _selectedAnimals.Count;
            _notificationStore.AddNotification($"{length} breeding events were successfully created for the synchronized animals.");
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
        private SynchronizationEvent GetSynchronizationEventFromFields()
        {
            return new SynchronizationEvent(1,
                                            _dateConverterService.GetDateOnly(_startDate),
                                            _endDate,
                                            _batchNumber,
                                            _synchronizationProtocol,
                                            _comments);
        }

        /// <summary>
        ///     Gets a breeding event from the form fields for a specific animal.
        /// </summary>
        /// <param name="animalId" >The ID of the animal.</param>
        /// <returns>A new breeding event.</returns>
        private BreedingEvent GetBreedingEventFromFields( int animalId )
        {
            return new BreedingEvent(1, animalId, AiDate, AiDate.AddDays(114), _synchronizationId);
        }

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
