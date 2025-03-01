using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Core.Validation;
using PiggsCare.Domain.Models;
using System.Collections;
using System.ComponentModel;

namespace PiggsCare.Core.ViewModels.HealthRecords
{
    public class HealthRecordModifyFormViewModel:MvxViewModel<int>, IHealthRecordModifyFormViewModel, INotifyDataErrorInfo
    {
        public IEnumerable GetErrors( string? propertyName )
        {
            return _recordValidation.GetErrors(propertyName);
        }

        public bool HasErrors => _recordValidation.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        #region Constructor

        public HealthRecordModifyFormViewModel( IHealthRecordStore healthRecordStore, ModalNavigationStore modalNavigationStore, IHealthRecordValidation recordValidation )
        {
            _healthRecordStore = healthRecordStore;
            _modalNavigationStore = modalNavigationStore;
            _recordValidation = recordValidation;
            _recordValidation.Errors.Clear();

            recordValidation.ErrorsChanged += RecordValidationOnErrorsChanged;
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
            HealthRecord? record = _healthRecordStore?.HealthRecords.FirstOrDefault(x => x.HealthRecordId == _healthRecordId);
            if (record == null) return base.Initialize();
            PopulateEditForm(record);
            _animalId = record.AnimalId;
            return base.Initialize();
        }

        #endregion

        #region Fields

        private DateOnly _recordDate;
        private string _diagnosis = string.Empty;
        private string _treatment = string.Empty;
        private string _outcome = string.Empty;
        private int _healthRecordId;
        private int _animalId;
        private readonly IHealthRecordStore _healthRecordStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly IHealthRecordValidation _recordValidation;

        #endregion

        #region Properties

        public DateOnly RecordDate
        {
            get => _recordDate;
            set
            {
                if (value.Equals(_recordDate)) return;
                _recordDate = value;
                _recordValidation.ValidateProp(value);
                RaisePropertyChanged();
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
            }
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand SubmitRecordCommand => new MvxAsyncCommand(ExecuteSubmitRecord, CanSubmitRecord);


        public IMvxCommand CancelRecordCommand => new MvxCommand(ExecuteCancelCommand);

        #endregion

        #region Methods

        private bool CanSubmitRecord()
        {
            return !HasErrors;
        }

        private void PopulateEditForm( HealthRecord record )
        {
            _recordDate = record.RecordDate;
            _diagnosis = record.Diagnosis;
            _treatment = record.Treatment;
            _outcome = record.Outcome;
        }

        private async Task ExecuteSubmitRecord()
        {
            HealthRecord record = GetHealthRecordFromFields();
            await _healthRecordStore.Modify(record);
            _modalNavigationStore.Close();
        }

        private HealthRecord GetHealthRecordFromFields()
        {
            return new HealthRecord(_healthRecordId, _animalId, RecordDate, _diagnosis, _treatment, _outcome);
            // Todo: use inserted date in both create instead of the DateTime object
        }

        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        #endregion
    }
}
