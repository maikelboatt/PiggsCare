using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Core.Validation;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;
using System.Collections;
using System.ComponentModel;

namespace PiggsCare.Core.ViewModels.Weaning
{
    public class WeaningCreateFormViewModel:MvxViewModel<int>, IWeaningCreateFormViewModel, INotifyDataErrorInfo
    {
        #region INotifyDataErrorInfo Implementation

        public IEnumerable GetErrors( string? propertyName )
        {
            return _recordValidation.GetErrors(propertyName);
        }

        public bool HasErrors => _recordValidation.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        #endregion

        #region Constructor

        public WeaningCreateFormViewModel( IWeaningStore weaningStore, ModalNavigationStore modalNavigationStore, IDateConverterService dateConverterService,
            IWeaningRecordValidation recordValidation )
        {
            _weaningStore = weaningStore;
            _modalNavigationStore = modalNavigationStore;
            _dateConverterService = dateConverterService;
            _recordValidation = recordValidation;
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
            SubmitRecordCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region ViewModel Life-Cycle

        public override void Prepare( int parameter )
        {
            _farrowingEventId = parameter;
        }

        public override void ViewDestroy( bool viewFinishing = true )
        {
            base.ViewDestroy(viewFinishing);
            _recordValidation.ErrorsChanged -= RecordValidationOnErrorsChanged;
        }

        #endregion

        #region Properties

        public DateTime WeaningDate
        {
            get => _weaningDate;
            set
            {
                if (value.Equals(_weaningDate)) return;
                _weaningDate = value;
                DateOnly outcome = _dateConverterService.GetDateOnly(value);
                _recordValidation.ValidateProp(outcome);
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }
        public int NumberWeaned
        {
            get => _numberWeaned;
            set
            {
                if (value == _numberWeaned) return;
                _numberWeaned = value;
                _recordValidation.ValidateProp(value);
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }
        public int MalesWeaned
        {
            get => _malesWeaned;
            set
            {
                if (value == _malesWeaned) return;
                _malesWeaned = value;
                _recordValidation.ValidateProp(value);
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }
        public int FemalesWeaned
        {
            get => _femalesWeaned;
            set
            {
                if (value == _femalesWeaned) return;
                _femalesWeaned = value;
                _recordValidation.ValidateProp(value);
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }
        public float AverageWeaningWeight
        {
            get => _averageWeaningWeight;
            set
            {
                if (value.Equals(_averageWeaningWeight)) return;
                _averageWeaningWeight = value;
                _recordValidation.ValidateProp(value);
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Fields

        private int _farrowingEventId;
        private DateTime _weaningDate;
        private int _numberWeaned;
        private int _malesWeaned;
        private int _femalesWeaned;
        private float _averageWeaningWeight;

        private readonly IDateConverterService _dateConverterService;
        private readonly IWeaningRecordValidation _recordValidation;
        private readonly IWeaningStore _weaningStore;
        private readonly ModalNavigationStore _modalNavigationStore;

        #endregion

        #region Commands

        public IMvxAsyncCommand SubmitRecordCommand { get; }

        private bool CanSubmitRecord()
        {
            bool noFieldEmpty = !WeaningDate.Equals(default);
            return noFieldEmpty && !HasErrors;
        }

        public IMvxCommand CancelRecordCommand { get; }

        #endregion

        #region Methods

        private async Task ExecuteSubmitRecord()
        {
            WeaningEvent record = GetWeaningEventsFromFields();
            await _weaningStore.Create(record);
            _modalNavigationStore.Close();
        }

        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        private WeaningEvent GetWeaningEventsFromFields()
        {
            return new WeaningEvent(1, _farrowingEventId, _dateConverterService.GetDateOnly(WeaningDate), NumberWeaned, MalesWeaned, FemalesWeaned, AverageWeaningWeight);
        }

        #endregion
    }
}
