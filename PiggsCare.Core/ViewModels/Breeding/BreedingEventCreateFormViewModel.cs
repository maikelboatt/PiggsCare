using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;

namespace PiggsCare.Core.ViewModels.Breeding
{
    public class BreedingEventCreateFormViewModel:MvxViewModel<int>, IBreedingEventCreateFormViewModel
    {
        #region Constructor

        public BreedingEventCreateFormViewModel( IBreedingEventStore breedingEventStore, ModalNavigationStore modalNavigationStore, IDateConverterService dateConverterService )
        {
            _breedingEventStore = breedingEventStore;
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

        #region Fields

        private DateTime _aiDate;
        private DateOnly _expectedFarrowDate;
        private int _animalId;
        private readonly IBreedingEventStore _breedingEventStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly IDateConverterService _dateConverterService;

        private const int GestationPeriod = 114;

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
            await _breedingEventStore.Create(record);
            _modalNavigationStore.Close();
        }

        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        private BreedingEvent GetBreedingEventFromFields()
        {
            return new BreedingEvent(1, _animalId, _dateConverterService.GetDateOnly(_aiDate), _expectedFarrowDate, null);
        }

        private void CalculateExpectedFarDate()
        {

            DateTime result = AiDate.AddDays(GestationPeriod);
            ExpectedFarrowDate = _dateConverterService.GetDateOnly(result);
        }

        #endregion
    }
}
