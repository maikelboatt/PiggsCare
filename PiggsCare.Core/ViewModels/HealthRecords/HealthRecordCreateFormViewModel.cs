using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Core.Validation;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;
using System.Collections;
using System.ComponentModel;

namespace PiggsCare.Core.ViewModels.HealthRecords
{
    public class HealthRecordCreateFormViewModel:MvxViewModel<int>, IHealthRecordCreateFormViewModel, INotifyDataErrorInfo
    {
        #region Constructor

        public HealthRecordCreateFormViewModel( IHealthRecordStore healthRecordStore, ModalNavigationStore modalNavigationStore, IHealthRecordValidation recordValidation,
            IDateConverterService dateConverterService )
        {
            _healthRecordStore = healthRecordStore;
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

        #region Fields

        private DateTime _recordDate;
        private string _diagnosis = string.Empty;
        private string _treatment = string.Empty;
        private string _outcome = string.Empty;
        private int _animalId;
        private readonly IHealthRecordStore _healthRecordStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly IHealthRecordValidation _recordValidation;
        private readonly IDateConverterService _dateConverterService;

        #endregion

        #region Properties

        public DateTime RecordDate
        {
            get => _recordDate;
            set
            {
                if (value.Equals(_recordDate)) return;
                _recordDate = value;
                _recordValidation.ValidateProp(_dateConverterService.GetDateOnly(_recordDate));
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }

        public string Diagnosis
        {
            get => _diagnosis;
            set
            {
                if (value.Equals(_diagnosis)) return;
                _diagnosis = value;
                _recordValidation.ValidateProp(value);
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }
        public string Treatment
        {
            get => _treatment;
            set
            {
                if (value.Equals(_treatment)) return;
                _treatment = value;
                _recordValidation.ValidateProp(_treatment);
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }
        public string Outcome
        {
            get => _outcome;
            set
            {
                if (value.Equals(_outcome)) return;
                _outcome = value;
                _recordValidation.ValidateProp(_outcome);
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand SubmitRecordCommand { get; }

        private bool CanSubmitRecord()
        {
            bool noFieldEmpty = !string.IsNullOrWhiteSpace(Diagnosis) && !string.IsNullOrWhiteSpace(Treatment) && !string.IsNullOrWhiteSpace(Outcome) && !RecordDate.Equals(default);
            return noFieldEmpty && !HasErrors;
        }

        public IMvxCommand CancelRecordCommand { get; }

        #endregion

        #region Methods

        private async Task ExecuteSubmitRecord()
        {
            HealthRecord record = GetHealthRecordFromFields();
            await _healthRecordStore.Create(record);
            _modalNavigationStore.Close();
        }

        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        private HealthRecord GetHealthRecordFromFields()
        {
            return new HealthRecord(1, _animalId, _dateConverterService.GetDateOnly(RecordDate), _diagnosis, _treatment, _outcome);
        }

        #endregion
    }
}
