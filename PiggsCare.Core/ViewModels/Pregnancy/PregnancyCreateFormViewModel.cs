using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores;
using PiggsCare.Business.Services.Pregnancy;
using PiggsCare.Core.Validation;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Services;
using System.Collections;
using System.ComponentModel;

namespace PiggsCare.Core.ViewModels.Pregnancy
{
    public class PregnancyCreateFormViewModel:MvxViewModel<int>, IPregnancyCreateFormViewModel, INotifyDataErrorInfo
    {
        private readonly IDateConverterService _dateConverterService;
        private readonly ModalNavigationStore _modalNavigationStore;


        private readonly IPregnancyService _pregnancyService;
        private readonly IPregnancyRecordValidation _recordValidation = new PregnancyRecordValidation();
        private int _breedingEventId;
        private DateTime _scanDate = DateTime.Now;
        private string _scanResults = string.Empty;

        #region Constructor

        public PregnancyCreateFormViewModel( IPregnancyService pregnancyService, ModalNavigationStore modalNavigationStore, IDateConverterService dateConverterService )
        {
            _pregnancyService = pregnancyService;
            _modalNavigationStore = modalNavigationStore;
            _dateConverterService = dateConverterService;

            _recordValidation.ErrorsChanged += RecordValidationOnErrorsChanged;

            // Initialize commands once so that RaiseCanExecuteChanged works as expected
            SubmitRecordCommand = new MvxAsyncCommand(ExecuteSubmitRecord, CanSubmitRecord);
            CancelRecordCommand = new MvxCommand(ExecuteCancelCommand);
        }

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
            _breedingEventId = parameter;
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

        #endregion


        #region Properties

        public DateTime ScanDate
        {
            get => _scanDate;
            set
            {
                if (value.Equals(_scanDate)) return;
                _scanDate = value;
                DateOnly outcome = _dateConverterService.GetDateOnly(value);
                _recordValidation.ValidateProp(outcome);
                RaisePropertyChanged(() => ScanDate);
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }
        public string ScanResults
        {
            get => _scanResults;
            set
            {
                if (value == _scanResults) return;
                _scanResults = value;
                _recordValidation.ValidateProp(value);
                RaisePropertyChanged(() => ScanResults);
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand SubmitRecordCommand { get; }

        private bool CanSubmitRecord()
        {
            bool noFieldEmpty = !string.IsNullOrWhiteSpace(ScanResults) && !ScanDate.Equals(default);
            return noFieldEmpty && !HasErrors;
        }

        public IMvxCommand CancelRecordCommand { get; }

        #endregion

        #region Methods

        private async Task ExecuteSubmitRecord()
        {
            PregnancyScan record = GetPregnancyScanFromFields();
            await _pregnancyService.CreatePregnancyScanAsync(record);
            _modalNavigationStore.Close();
        }

        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        private PregnancyScan GetPregnancyScanFromFields() => new(1, _breedingEventId, _dateConverterService.GetDateOnly(ScanDate), ScanResults);

        #endregion
    }
}
