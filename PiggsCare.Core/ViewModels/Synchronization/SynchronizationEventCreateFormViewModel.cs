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
    public class SynchronizationEventCreateFormViewModel:MvxViewModel<List<Animal>>, ISynchronizationEventCreateFormViewModel
    {
        #region Constructor

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

        public override void Prepare( List<Animal> parameter )
        {
            _selectedAnimals = parameter;
        }

        public override void ViewDestroy( bool viewFinishing = true )
        {
            base.ViewDestroy(viewFinishing);
            _recordValidation.ErrorsChanged -= RecordValidationOnErrorsChanged;
        }

        #endregion

        #region INotifyDataErrorInfo Implementation

        public IEnumerable GetErrors( string? propertyName )
        {
            return _recordValidation.GetErrors(propertyName);
        }

        public bool HasErrors => _recordValidation.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

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

        public IMvxAsyncCommand SubmitRecordCommand { get; }

        private bool CanSubmitRecord()
        {
            bool noFieldEmpty = !string.IsNullOrWhiteSpace(BatchNumber) && !string.IsNullOrWhiteSpace(SynchronizationProtocol) && !string.IsNullOrWhiteSpace(Comments) &&
                                !StartDate.Equals(default) && !EndDate.Equals(default);
            return noFieldEmpty && !HasErrors;
        }

        public IMvxCommand CancelRecordCommand { get; }

        #endregion

        #region Methods

        private async Task ExecuteSubmitRecord()
        {
            SynchronizationEvent record = GetSynchronizationEventFromFields();
            _synchronizationId = await _synchronizationEventStore.CreateAsync(record);
            ConfirmInsemination();
            _modalNavigationStore.Close();
        }

        private void ConfirmInsemination()
        {
            MessageBoxResult result = _messageService.Show("Do you want to create an Insemination Event for these records?",
                                                           "Confirmation",
                                                           MessageBoxButton.YesNo,
                                                           MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                CreateInseminationRecordForSynchronizedAnimals();
        }

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

        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        private SynchronizationEvent GetSynchronizationEventFromFields()
        {
            return new SynchronizationEvent(1,
                                            _dateConverterService.GetDateOnly(_startDate),
                                            _endDate,
                                            _batchNumber,
                                            _synchronizationProtocol,
                                            _comments);
        }

        private BreedingEvent GetBreedingEventFromFields( int animalId )
        {
            return new BreedingEvent(1, animalId, AiDate, AiDate.AddDays(114), _synchronizationId);
        }

        private void CalculateEndDate()
        {
            DateTime result = StartDate.AddDays(SynchronizationPeriodInDays);
            EndDate = _dateConverterService.GetDateOnly(result);
        }

        #endregion
    }
}
