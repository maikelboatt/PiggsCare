using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores;
using PiggsCare.Business.Services.Removal;
using PiggsCare.Core.Validation;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Services;
using System.Collections;
using System.ComponentModel;

namespace PiggsCare.Core.ViewModels.Removal
{
    public class RemovalEventModifyFormViewModel:MvxViewModel<int>, IRemovalEventModifyFormViewModel, INotifyDataErrorInfo
    {
        #region Constructor

        public RemovalEventModifyFormViewModel( IRemovalService removalService, ModalNavigationStore modalNavigationStore, IDateConverterService dateConverterService,
            IRemovalEventValidation recordValidation )
        {
            _removalService = removalService;
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

        #region ViewModel Life-Cycle

        public override void Prepare( int parameter )
        {
            _removalId = parameter;
        }

        public override Task Initialize()
        {
            RemovalEvent? record = _removalService.GetRemovalEventByIdAsync(_removalId);
            if (record is null) return base.Initialize();
            PopulateModifyForm(record);
            _animalId = record.AnimalId;
            return base.Initialize();
        }

        public override void ViewDestroy( bool viewFinishing = true )
        {
            base.ViewDestroy(viewFinishing);
            _recordValidation.ErrorsChanged -= RecordValidationOnErrorsChanged;
        }

        #endregion

        #region INotifyDataErrorInfo Implementation

        public IEnumerable GetErrors( string? propertyName ) => _recordValidation.GetErrors(propertyName);

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
        private readonly IRemovalService _removalService;
        private readonly ModalNavigationStore _modalNavigationStore;
        private int _removalId;
        private int _animalId;
        private DateTime _removalDate;
        private string _reasonForRemoval = string.Empty;

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
                if (value == _reasonForRemoval) return;
                _reasonForRemoval = value;
                _recordValidation.ValidateProp(_reasonForRemoval);
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }

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

        private void PopulateModifyForm( RemovalEvent record )
        {
            _removalId = record.RemovalEventId;
            _animalId = record.AnimalId;
            _removalDate = _dateConverterService.GetDateTime(record.RemovalDate);
            _reasonForRemoval = record.ReasonForRemoval;
        }

        private async Task ExecuteSubmitRecord()
        {
            RemovalEvent record = GetRemovalEventFromFields();
            await _removalService.UpdateRemovalEventAsync(record);
            _modalNavigationStore.Close();
        }

        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        private RemovalEvent GetRemovalEventFromFields() => new(_removalId, _animalId, _dateConverterService.GetDateOnly(_removalDate), _reasonForRemoval);

        #endregion
    }
}
