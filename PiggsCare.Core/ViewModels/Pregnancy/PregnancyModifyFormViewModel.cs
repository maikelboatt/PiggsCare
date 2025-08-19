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
    public class PregnancyModifyFormViewModel:MvxViewModel<int>, IPregnancyModifyFormViewModel, INotifyDataErrorInfo
    {
        #region INotifyDataErrorInfo Implementation

        public IEnumerable GetErrors( string? propertyName ) => _recordValidation.GetErrors(propertyName);

        public bool HasErrors => _recordValidation.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        #endregion

        #region Constructor

        public PregnancyModifyFormViewModel( IPregnancyService pregnancyService, ModalNavigationStore modalNavigationStore, IDateConverterService dateConverterService,
            IPregnancyRecordValidation recordValidation )
        {
            _pregnancyService = pregnancyService;
            _modalNavigationStore = modalNavigationStore;
            _dateConverterService = dateConverterService;
            _recordValidation = recordValidation;

            recordValidation.ErrorsChanged += RecordValidationOnErrorsChanged;

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
            _pregnancyScanId = parameter;
        }

        public override Task Initialize()
        {
            PregnancyScan? record = _pregnancyService.GetPregnancyScanByIdAsync(_pregnancyScanId);
            if (record == null) return base.Initialize();
            PopulateEditForm(record);
            _breedingEventId = record.BreedingEventId;
            return base.Initialize();
        }

        public override void ViewDestroy( bool viewFinishing = true )
        {
            base.ViewDestroy(viewFinishing);
            _recordValidation.ErrorsChanged -= RecordValidationOnErrorsChanged;
        }

        #endregion

        #region Fields

        private readonly IPregnancyService _pregnancyService;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly IDateConverterService _dateConverterService;
        private readonly IPregnancyRecordValidation _recordValidation;

        private int _breedingEventId;
        private int _pregnancyScanId;
        private DateTime _scanDate;
        private string _scanResults;

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

        private void PopulateEditForm( PregnancyScan scan )
        {
            _scanDate = _dateConverterService.GetDateTime(scan.ScanDate);
            _scanResults = scan.ScanResults;
        }

        private async Task ExecuteSubmitRecord()
        {
            PregnancyScan record = GetPregnancyScanFromFields();
            await _pregnancyService.UpdatePregnancyScanAsync(record);
            _modalNavigationStore.Close();
        }

        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        private PregnancyScan GetPregnancyScanFromFields() => new(_pregnancyScanId, _breedingEventId, _dateConverterService.GetDateOnly(ScanDate), ScanResults);

        #endregion
    }
}
