using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;

namespace PiggsCare.Core.ViewModels.Breeding
{
    public class BreedingEventModifyFormViewModel( IBreedingEventStore breedingEventStore, ModalNavigationStore modalNavigationStore, IDateConverterService dateConverterService )
        :MvxViewModel<int>, IBreedingEventModifyFormViewModel
    {
        public override Task Initialize()
        {
            BreedingEvent? record = breedingEventStore?.BreedingEvents.FirstOrDefault(x => x.BreedingEventId == _breedingEventId);
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
            _aiDate = dateConverterService.GetDateTime(breedingEvent.AiDate);
            _expectedFarrowDate = breedingEvent.ExpectedFarrowDate;
        }

        #region Fields

        private DateTime _aiDate;
        private DateOnly _expectedFarrowDate;
        private int _animalId;
        private int _breedingEventId;
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
            }
        }
        public DateOnly ExpectedFarrowDate
        {
            get => _expectedFarrowDate;
            set => SetProperty(ref _expectedFarrowDate, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand SubmitRecordCommand => new MvxAsyncCommand(ExecuteSubmitRecord);

        public IMvxCommand CancelRecordCommand => new MvxCommand(ExecuteCancelCommand);

        #endregion


        #region Methods

        private async Task ExecuteSubmitRecord()
        {
            BreedingEvent record = GetBreedingEventFromFields();
            await breedingEventStore.Modify(record);
            modalNavigationStore.Close();
        }

        private void ExecuteCancelCommand()
        {
            modalNavigationStore.Close();
        }

        private BreedingEvent GetBreedingEventFromFields()
        {
            return new BreedingEvent(_breedingEventId, _animalId, dateConverterService.GetDateOnly(_aiDate), _expectedFarrowDate);
        }

        private void CalculateExpectedFarDate()
        {
            DateTime result = AiDate.AddDays(GestationPeriod);
            ExpectedFarrowDate = dateConverterService.GetDateOnly(result);
        }

        #endregion
    }
}
