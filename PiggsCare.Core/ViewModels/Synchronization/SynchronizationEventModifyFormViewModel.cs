using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Core.Validation;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;
using System.Collections;
using System.ComponentModel;

namespace PiggsCare.Core.ViewModels.Synchronization
{
    public class SynchronizationEventModifyFormViewModel:MvxViewModel<int>, ISynchronizationEventModifyFormViewModel
    {
        #region Constructor

        public SynchronizationEventModifyFormViewModel( ISynchronizationEventStore synchronizationEventStore, ModalNavigationStore modalNavigationStore,
            ISynchronizationRecordValidation recordValidation, IDateConverterService dateConverterService )
        {
            _synchronizationEventStore = synchronizationEventStore;
            _modalNavigationStore = modalNavigationStore;
            _recordValidation = recordValidation;
            _dateConverterService = dateConverterService;
            _recordValidation.Errors.Clear();

            recordValidation.ErrorsChanged += RecordValidationOnErrorsChanged;

            // Initialize commands once so that RaiseCanExecuteChanged works as expected
            SubmitRecordCommand = new MvxAsyncCommand(ExecuteSubmitRecord, CanSubmitRecord);
            CancelRecordCommand = new MvxCommand(ExecuteCancelCommand);
        }

        #endregion

        #region ViewModel Life-Cycle

        public override void Prepare( int parameter )
        {
            _synchronizationId = parameter;
        }

        public override Task Initialize()
        {
            SynchronizationEvent? record = _synchronizationEventStore.SynchronizationEvents.FirstOrDefault(x => x.SynchronizationEventId == _synchronizationId);
            if (record is null) return base.Initialize();
            PopulateModifyForm(record);
            return base.Initialize();
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
        private int _synchronizationId;

        private readonly ISynchronizationEventStore _synchronizationEventStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly ISynchronizationRecordValidation _recordValidation;
        private readonly IDateConverterService _dateConverterService;

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
            await _synchronizationEventStore.ModifyAsync(record);
            _modalNavigationStore.Close();
        }

        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        private void PopulateModifyForm( SynchronizationEvent synchronizationEvent )
        {
            _synchronizationId = synchronizationEvent.SynchronizationEventId;
            _startDate = _dateConverterService.GetDateTime(synchronizationEvent.StartDate);
            _endDate = synchronizationEvent.EndDate;
            _batchNumber = synchronizationEvent.BatchNumber;
            _synchronizationProtocol = synchronizationEvent.SynchronizationProtocol;
            _comments = synchronizationEvent.Comments;
        }

        private SynchronizationEvent GetSynchronizationEventFromFields()
        {
            return new SynchronizationEvent(_synchronizationId,
                                            _dateConverterService.GetDateOnly(_startDate),
                                            _endDate,
                                            _batchNumber,
                                            _synchronizationProtocol,
                                            _comments);
        }

        private void CalculateEndDate()
        {
            DateTime result = StartDate.AddDays(SynchronizationPeriodInDays);
            EndDate = _dateConverterService.GetDateOnly(result);
        }

        #endregion
    }
}
