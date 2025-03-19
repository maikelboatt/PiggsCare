using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;

namespace PiggsCare.Core.ViewModels.Breeding
{
    public class BreedingEventModifyFormViewModel:MvxViewModel<int>, IBreedingEventModifyFormViewModel
    {
        #region Constructor

        public BreedingEventModifyFormViewModel( IBreedingEventStore breedingEventStore, ModalNavigationStore modalNavigationStore, IDateConverterService dateConverterService )
        {
            _breedingEventStore = breedingEventStore;
            _modalNavigationStore = modalNavigationStore;
            _dateConverterService = dateConverterService;

            // Initialize commands once so that RaiseCanExecuteChanged works as expected
            SubmitRecordCommand = new MvxAsyncCommand(ExecuteSubmitRecord, CanSubmitRecord);
            CancelRecordCommand = new MvxCommand(ExecuteCancelCommand);
        }

        #endregion

        public override Task Initialize()
        {
            BreedingEvent? record = _breedingEventStore?.BreedingEvents.FirstOrDefault(x => x.BreedingEventId == _breedingEventId);
            if (record == null) return base.Initialize();
            PopulateModifyForm(record);
            _animalId = record.AnimalId;
            return base.Initialize();
        }

        public override void Prepare( int parameter )
        {
            _breedingEventId = parameter;
        }

        private void PopulateModifyForm( BreedingEvent breedingEvent )
        {
            _breedingEventId = breedingEvent.BreedingEventId;
            _animalId = breedingEvent.AnimalId;
            _aiDate = _dateConverterService.GetDateTime(breedingEvent.AiDate);
            _expectedFarrowDate = breedingEvent.ExpectedFarrowDate;
        }

        #region Fields

        private DateTime _aiDate;
        private DateOnly _expectedFarrowDate;
        private int _animalId;
        private int _breedingEventId;
        private readonly IBreedingEventStore _breedingEventStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly IDateConverterService _dateConverterService;


        private const int GestationPeriod = 114;

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
            BreedingEvent record = GetBreedingEventFromFields();
            await _breedingEventStore.Modify(record);
            _modalNavigationStore.Close();
        }

        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        private BreedingEvent GetBreedingEventFromFields()
        {
            return new BreedingEvent(_breedingEventId, _animalId, _dateConverterService.GetDateOnly(_aiDate), _expectedFarrowDate, 0);
        }

        private void CalculateExpectedFarDate()
        {
            DateTime result = AiDate.AddDays(GestationPeriod);
            ExpectedFarrowDate = _dateConverterService.GetDateOnly(result);
        }

        #endregion
    }
}
