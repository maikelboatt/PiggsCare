using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Core.Validation;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;
using System.Collections;
using System.ComponentModel;

namespace PiggsCare.Core.ViewModels.Farrowing
{
    public class FarrowingModifyFormViewModel:MvxViewModel<int>, IFarrowingModifyFormViewModel, INotifyDataErrorInfo
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

        public FarrowingModifyFormViewModel( IFarrowingStore farrowingStore, ModalNavigationStore modalNavigationStore, IDateConverterService dateConverterService,
            IFarrowRecordValidation recordValidation )
        {
            _farrowingStore = farrowingStore;
            _modalNavigationStore = modalNavigationStore;
            _dateConverterService = dateConverterService;
            _recordValidation = recordValidation;
            _recordValidation.Errors.Clear();

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

        #region Methods

        private void PopulateEditForm( FarrowEvent record )
        {
            _farrowDate = _dateConverterService.GetDateTime(record.FarrowDate);
            _litterSize = record.LitterSize;
            _bornAlive = record.BornAlive;
            _bornDead = record.BordDead;
            _mummified = record.Mummified;
        }

        private async Task ExecuteSubmitRecord()
        {
            FarrowEvent record = GetFarrowFromFields();
            await _farrowingStore.Modify(record);
            _modalNavigationStore.Close();
        }

        private FarrowEvent GetFarrowFromFields()
        {
            return new FarrowEvent(_farrowingEventId, _breedingEventId, _dateConverterService.GetDateOnly(FarrowDate), LitterSize, BornAlive, BornDead, Mummified);
        }

        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        #endregion

        #region ViewModel Life-Cycle

        public override void Prepare( int parameter )
        {
            _farrowingEventId = parameter;
        }

        public override Task Initialize()
        {
            FarrowEvent? record = _farrowingStore.FarrowEvents.FirstOrDefault(x => x.FarrowingEventId == _farrowingEventId);
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

        private int _farrowingEventId;
        private int _breedingEventId;
        private string _birthStatus = string.Empty;
        private DateTime _farrowDate;
        private int _litterSize;
        private int _bornAlive;
        private int _bornDead;
        private int _mummified;

        private readonly IFarrowingStore _farrowingStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly IDateConverterService _dateConverterService;
        private readonly IFarrowRecordValidation _recordValidation;

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
                if (value == _litterSize) return;
                _litterSize = value;
                _recordValidation.ValidateProp(value);
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

        public BirthStatus[] BirthStatusCollection => Enum.GetValues<BirthStatus>().ToArray();

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
    }
}
