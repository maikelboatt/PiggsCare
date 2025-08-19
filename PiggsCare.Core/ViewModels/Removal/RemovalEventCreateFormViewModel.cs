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
    public class RemovalEventCreateFormViewModel:MvxViewModel<int>, IRemovalEventCreateFormViewModel, INotifyDataErrorInfo
    {
        private readonly IDateConverterService _dateConverterService;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly IRemovalEventValidation _recordValidation = new RemovalEventValidation();
        private readonly IRemovalService _removalService;
        private int _animalId;
        private string _reasonForRemoval = string.Empty;
        private DateTime _removalDate = DateTime.Now;

        #region Constructor

        public RemovalEventCreateFormViewModel( IRemovalService removalService, ModalNavigationStore modalNavigationStore, IDateConverterService dateConverterService )
        {
            _removalService = removalService;
            _modalNavigationStore = modalNavigationStore;
            _dateConverterService = dateConverterService;

            _recordValidation.ErrorsChanged += RecordValidationOnErrorsChanged;

            // Initialize commands once so that RaiseCanExecuteChanged works as expected
            SubmitRecordCommand = new MvxAsyncCommand(ExecuteSubmitRecord, CanSubmitRecord);
            CancelRecordCommand = new MvxCommand(ExecuteCancelCommand);
        }

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
                // if (value == _reasonForRemoval) return;
                _reasonForRemoval = value;
                _recordValidation.ValidateProp(_reasonForRemoval);
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
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

        private async Task ExecuteSubmitRecord()
        {
            RemovalEvent record = GetRemovalEventFromFields();
            await _removalService.CreateRemovalEventAsync(record);
            _modalNavigationStore.Close();
        }

        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        private RemovalEvent GetRemovalEventFromFields() => new(1, _animalId, _dateConverterService.GetDateOnly(_removalDate), _reasonForRemoval);

        #endregion
    }
}
