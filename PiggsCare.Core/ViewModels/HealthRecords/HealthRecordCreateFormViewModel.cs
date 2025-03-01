using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Core.Validation;
using PiggsCare.Domain.Models;
using System.Collections;
using System.ComponentModel;

namespace PiggsCare.Core.ViewModels.HealthRecords
{
    public class HealthRecordCreateFormViewModel:MvxViewModel<int>, IHealthRecordCreateFormViewModel, INotifyDataErrorInfo
    {
        #region Constructor

        public HealthRecordCreateFormViewModel( IHealthRecordStore healthRecordStore, ModalNavigationStore modalNavigationStore, IHealthRecordValidation recordValidation )
        {
            _healthRecordStore = healthRecordStore;
            _modalNavigationStore = modalNavigationStore;
            _recordValidation = recordValidation;
            _recordValidation.Errors.Clear();

            recordValidation.ErrorsChanged += RecordValidationOnErrorsChanged;
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
            RaisePropertyChanged(nameof(CanSubmitRecord));
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

        private DateOnly _recordDate;
        private string _diagnosis = string.Empty;
        private string _treatment = string.Empty;
        private string _outcome = string.Empty;
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

        private bool CanSubmitRecord()
        {
            return !HasErrors;
        }

        public IMvxCommand CancelRecordCommand => new MvxCommand(ExecuteCancelCommand);

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
            return new HealthRecord(1, _animalId, RecordDate, _diagnosis, _treatment, _outcome);
        }

        #endregion
    }
}
