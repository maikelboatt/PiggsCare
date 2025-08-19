using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores;
using PiggsCare.Business.Services.Farrowing;
using PiggsCare.Core.Validation;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Services;
using System.Collections;
using System.ComponentModel;

namespace PiggsCare.Core.ViewModels.Farrowing
{
    public class FarrowingCreateFormViewModel:MvxViewModel<int>, IFarrowingCreateFormViewModel, INotifyDataErrorInfo
    {
        private readonly IDateConverterService _dateConverterService;

        private readonly IFarrowingService _farrowingService;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly IFarrowRecordValidation _recordValidation = new FarrowRecordValidation();
        private string _birthStatus = string.Empty;
        private int _bornAlive;
        private int _bornDead;

        private int _breedingEventId;
        private DateTime _farrowDate = DateTime.Now;
        private int _litterSize;
        private int _mummified;


        #region Constructor

        public FarrowingCreateFormViewModel( IFarrowingService farrowingService, ModalNavigationStore modalNavigationStore, IDateConverterService dateConverterService )
        {
            _farrowingService = farrowingService;
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

        public string BirthStatus
        {
            get => _birthStatus;
            set
            {
                if (value == _birthStatus) return;
                _birthStatus = value;
                _recordValidation.ValidateProp(value);
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }
        public DateTime FarrowDate
        {
            get => _farrowDate;
            set
            {
                if (value.Equals(_farrowDate)) return;
                _farrowDate = value;
                DateOnly outcome = _dateConverterService.GetDateOnly(value);
                _recordValidation.ValidateProp(outcome);
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }
        public int LitterSize
        {
            get => _litterSize;
            set
            {
                if (value.Equals(_litterSize)) return;
                _litterSize = value;
                _recordValidation.ValidateProp(_litterSize);
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }

        public int BornAlive
        {
            get => _bornAlive;
            set
            {
                if (value == _bornAlive) return;
                _bornAlive = value;
                _recordValidation.ValidateProp(_bornAlive);
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }
        public int BornDead
        {
            get => _bornDead;
            set
            {
                if (value == _bornDead) return;
                _bornDead = value;
                _recordValidation.ValidateProp(_bornDead);
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }
        public int Mummified
        {
            get => _mummified;
            set
            {
                if (value == _mummified) return;
                _mummified = value;
                _recordValidation.ValidateProp(_mummified);
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }

        public BirthStatus[] BirthStatusCollection => Enum.GetValues<BirthStatus>();

        #endregion


        #region Commands

        public IMvxAsyncCommand SubmitRecordCommand { get; }

        private bool CanSubmitRecord()
        {
            bool noFieldEmpty = !FarrowDate.Equals(default);
            // && !LitterSize.Equals(0) && !BornAlive.Equals(0) && !BornDead.Equals(0) &&  !Mummified.Equals(0);
            return noFieldEmpty && !HasErrors;
        }

        public IMvxCommand CancelRecordCommand { get; }

        #endregion

        #region Methods

        private async Task ExecuteSubmitRecord()
        {
            FarrowEvent record = GetFarrowFromFields();
            await _farrowingService.CreateFarrowingAsync(record);
            _modalNavigationStore.Close();
        }


        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        private FarrowEvent GetFarrowFromFields() => new(1, _breedingEventId, _dateConverterService.GetDateOnly(FarrowDate), LitterSize, BornAlive, BornDead, Mummified);

        #endregion
    }
}
