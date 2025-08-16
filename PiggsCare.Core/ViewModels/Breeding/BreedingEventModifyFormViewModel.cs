using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores;
using PiggsCare.Business.Services.Insemination;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Services;

namespace PiggsCare.Core.ViewModels.Breeding
{
    public class BreedingEventModifyFormViewModel:MvxViewModel<int>, IBreedingEventModifyFormViewModel
    {
        private const int GestationPeriod = 114;
        private readonly IDateConverterService _dateConverterService;
        private readonly IInseminationService _inseminationService;
        private readonly ModalNavigationStore _modalNavigationStore;


        private DateTime _aiDate;
        private int _animalId;
        private int _breedingEventId;
        private DateOnly _expectedFarrowDate;

        #region Constructor

        public BreedingEventModifyFormViewModel( IInseminationService inseminationService, ModalNavigationStore modalNavigationStore,
            IDateConverterService dateConverterService )
        {
            _inseminationService = inseminationService;
            _modalNavigationStore = modalNavigationStore;
            _dateConverterService = dateConverterService;

            // Initialize commands once so that RaiseCanExecuteChanged works as expected
            SubmitRecordCommand = new MvxAsyncCommand(ExecuteSubmitRecord, CanSubmitRecord);
            CancelRecordCommand = new MvxCommand(ExecuteCancelCommand);
        }

        #endregion

        public override Task Initialize()
        {
            InseminationEvent? record = _inseminationService.GetInseminationEventByIdAsync(_breedingEventId);
            if (record == null) return base.Initialize();
            PopulateModifyForm(record);
            _animalId = record.AnimalId;
            return base.Initialize();
        }

        public override void Prepare( int parameter )
        {
            _breedingEventId = parameter;
        }

        private void PopulateModifyForm( InseminationEvent inseminationEvent )
        {
            _breedingEventId = inseminationEvent.BreedingEventId;
            _animalId = inseminationEvent.AnimalId;
            _aiDate = _dateConverterService.GetDateTime(inseminationEvent.AiDate);
            _expectedFarrowDate = inseminationEvent.ExpectedFarrowDate;
        }


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
            await _inseminationService.UpdateInseminationEventAsync(record);
            _modalNavigationStore.Close();
        }

        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        private InseminationEvent GetBreedingEventFromFields() => new(_breedingEventId, _animalId, _dateConverterService.GetDateOnly(_aiDate), _expectedFarrowDate, 0);

        private void CalculateExpectedFarDate()
        {
            DateTime result = AiDate.AddDays(GestationPeriod);
            ExpectedFarrowDate = _dateConverterService.GetDateOnly(result);
        }

        #endregion
    }
}
