using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores;
using PiggsCare.Business.Services.Insemination;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Services;

namespace PiggsCare.Core.ViewModels.Breeding
{
    public class BreedingEventCreateFormViewModel:MvxViewModel<int>, IBreedingEventCreateFormViewModel
    {
        private const int GestationPeriod = 114;
        private readonly IDateConverterService _dateConverterService;
        private readonly IInseminationService _inseminationService;
        private readonly ModalNavigationStore _modalNavigationStore;

        private DateTime _aiDate;
        private int _animalId;
        private DateOnly _expectedFarrowDate = DateOnly.FromDateTime(DateTime.Now);

        #region Constructor

        public BreedingEventCreateFormViewModel( IInseminationService inseminationService, ModalNavigationStore modalNavigationStore, IDateConverterService dateConverterService )
        {
            _inseminationService = inseminationService;
            _modalNavigationStore = modalNavigationStore;
            _dateConverterService = dateConverterService;

            // Initialize commands once so that RaiseCanExecuteChanged works as expected
            SubmitRecordCommand = new MvxAsyncCommand(ExecuteSubmitRecord, CanSubmitRecord);
            CancelRecordCommand = new MvxCommand(ExecuteCancelCommand);
        }

        #endregion

        #region ViewModel Life Cycle

        public override void Prepare( int parameter )
        {
            _animalId = parameter;
        }

        #endregion

        #region Properties

        public DateTime AiDate
        {
            get => _aiDate;
            set
            {
                if (value.Equals(_aiDate)) return;
                _aiDate = value;
                RaisePropertyChanged();
                CalculateExpectedFarDate();
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }
        public DateOnly ExpectedFarrowDate
        {
            get => _expectedFarrowDate;
            set => SetProperty(ref _expectedFarrowDate, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand SubmitRecordCommand { get; }

        private bool CanSubmitRecord()
        {
            bool noFieldEmpty = !ExpectedFarrowDate.Equals(default) && !AiDate.Equals(default);
            return noFieldEmpty;
        }

        public IMvxCommand CancelRecordCommand { get; }

        #endregion

        #region Methods

        private async Task ExecuteSubmitRecord()
        {
            InseminationEvent record = GetBreedingEventFromFields();
            await _inseminationService.CreateInseminationEventAsync(record);
            _modalNavigationStore.Close();
        }

        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        private InseminationEvent GetBreedingEventFromFields() => new(1, _animalId, _dateConverterService.GetDateOnly(_aiDate), _expectedFarrowDate, null);

        private void CalculateExpectedFarDate()
        {

            DateTime result = AiDate.AddDays(GestationPeriod);
            ExpectedFarrowDate = _dateConverterService.GetDateOnly(result);
        }

        #endregion
    }
}
