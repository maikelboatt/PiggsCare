using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores;
using PiggsCare.Business.Services.Health;
using PiggsCare.Core.Validation;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Services;
using System.Collections;
using System.ComponentModel;

namespace PiggsCare.Core.ViewModels.HealthRecords
{
    public class HealthRecordModifyFormViewModel:MvxViewModel<int>, IHealthRecordModifyFormViewModel, INotifyDataErrorInfo
    {
        public IEnumerable GetErrors( string? propertyName ) => _recordValidation.GetErrors(propertyName);

        public bool HasErrors => _recordValidation.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        #region Constructor

        public HealthRecordModifyFormViewModel( IHealthService healthService, ModalNavigationStore modalNavigationStore, IHealthRecordValidation recordValidation,
            IDateConverterService dateConverterService )
        {
            _healthService = healthService;
            _modalNavigationStore = modalNavigationStore;
            _recordValidation = recordValidation;
            _dateConverterService = dateConverterService;
            _recordValidation.Errors.Clear();

            recordValidation.ErrorsChanged += RecordValidationOnErrorsChanged;

            // Initialize commands once so that RaiseCanExecuteChanged works as expected
            SubmitRecordCommand = new MvxAsyncCommand(ExecuteSubmitRecord, CanSubmitRecord);
            CancelRecordCommand = new MvxCommand(ExecuteCancelCommand);
        }

        private void RecordValidationOnErrorsChanged( object? sender, DataErrorsChangedEventArgs e )
        {
            ErrorsChanged?.Invoke(this, e);
            RaisePropertyChanged(nameof(HasErrors));
            RaisePropertyChanged(nameof(CanSubmitRecord));
        }

        #endregion

        #region ViewModel Life-Cycle

        public override void Prepare( int parameter )
        {
            _healthRecordId = parameter;
        }

        public override Task Initialize()
        {
            HealthRecord? record = _healthService.GetHealthRecordByIdAsync(_healthRecordId);
            if (record == null) return base.Initialize();
            PopulateEditForm(record);
            _animalId = record.AnimalId;
            return base.Initialize();
        }

        #endregion

        #region Fields

        private DateTime _recordDate;
        private string _diagnosis = string.Empty;
        private string _treatment = string.Empty;
        private string _outcome = string.Empty;
        private int _healthRecordId;
        private int _animalId;
        private readonly IHealthService _healthService;
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
                RaisePropertyChanged();
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

        private void PopulateEditForm( HealthRecord record )
        {
            _recordDate = _dateConverterService.GetDateTime(record.RecordDate);
            _diagnosis = record.Diagnosis;
            _treatment = record.Treatment;
            _outcome = record.Outcome;
        }

        private async Task ExecuteSubmitRecord()
        {
            HealthRecord record = GetHealthRecordFromFields();
            await _healthService.UpdateHealthRecordAsync(record);
            _modalNavigationStore.Close();
        }

        private HealthRecord GetHealthRecordFromFields() => new(_healthRecordId, _animalId, _dateConverterService.GetDateOnly(RecordDate), _diagnosis, _treatment, _outcome);

        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        #endregion
    }
}
