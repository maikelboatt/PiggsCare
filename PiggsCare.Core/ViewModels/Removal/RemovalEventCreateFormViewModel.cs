using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Core.Validation;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;
using System.Collections;
using System.ComponentModel;

namespace PiggsCare.Core.ViewModels.Removal
{
    public class RemovalEventCreateFormViewModel:MvxViewModel<int>, IRemovalEventCreateFormViewModel, INotifyDataErrorInfo
    {
        #region Constructor

        public RemovalEventCreateFormViewModel( IRemovalEventStore eventStore, ModalNavigationStore modalNavigationStore, IDateConverterService dateConverterService,
            IRemovalEventValidation recordValidation )
        {
            _eventStore = eventStore;
            _modalNavigationStore = modalNavigationStore;
            _dateConverterService = dateConverterService;
            _recordValidation = recordValidation;
            _recordValidation.Errors.Clear();

            recordValidation.ErrorsChanged += RecordValidationOnErrorsChanged;

            // Initialize commands once so that RaiseCanExecuteChanged works as expected
            SubmitRecordCommand = new MvxAsyncCommand(ExecuteSubmitRecord, CanSubmitRecord);
            CancelRecordCommand = new MvxCommand(ExecuteCancelCommand);
        }

        #endregion

        #region Properties

        public DateTime RemovalDate
        {
            get => _removalDate;
            set
            {
                if (value.Equals(_removalDate)) return;
                _removalDate = value;
                _recordValidation.ValidateProp(_dateConverterService.GetDateOnly(_removalDate));
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }
        public string ReasonForRemoval
        {
            get => _reasonForRemoval;
            set
            {
                // if (value == _reasonForRemoval) return;
                _reasonForRemoval = value;
                _recordValidation.ValidateProp(_reasonForRemoval);
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region ViewModel Life-Cycle

        public override void Prepare( int parameter )
        {
            _animalId = parameter;
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

        private readonly IDateConverterService _dateConverterService;
        private readonly IRemovalEventValidation _recordValidation;
        private readonly IRemovalEventStore _eventStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        private int _animalId;
        private DateTime _removalDate;
        private string _reasonForRemoval = string.Empty;

        #endregion

        #region Commands

        public IMvxAsyncCommand SubmitRecordCommand { get; }

        private bool CanSubmitRecord()
        {
            bool noFieldEmpty = !RemovalDate.Equals(default) && !string.IsNullOrWhiteSpace(ReasonForRemoval);
            return noFieldEmpty && !HasErrors;
        }

        public IMvxCommand CancelRecordCommand { get; }

        #endregion

        #region Methods

        private async Task ExecuteSubmitRecord()
        {
            RemovalEvent record = GetRemovalEventFromFields();
            await _eventStore.CreateAsync(record);
            _modalNavigationStore.Close();
        }

        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        private RemovalEvent GetRemovalEventFromFields()
        {
            return new RemovalEvent(1, _animalId, _dateConverterService.GetDateOnly(_removalDate), _reasonForRemoval);
        }

        #endregion
    }
}
