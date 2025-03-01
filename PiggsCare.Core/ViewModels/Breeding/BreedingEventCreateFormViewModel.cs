using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;

namespace PiggsCare.Core.ViewModels.Breeding
{
    public class BreedingEventCreateFormViewModel( IBreedingEventStore breedingEventStore, ModalNavigationStore modalNavigationStore, IDateConverterService dateConverterService )
        :MvxViewModel<int>, IBreedingEventCreateFormViewModel
    {
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
        private const int GestationPeriod = 114;

        #endregion

        #region Commands

        public IMvxAsyncCommand SubmitRecordCommand => new MvxAsyncCommand(ExecuteSubmitRecord);

        public IMvxCommand CancelRecordCommand => new MvxCommand(ExecuteCancelCommand);

        #endregion

        #region Methods

        private async Task ExecuteSubmitRecord()
        {
            BreedingEvent record = GetBreedingEventFromFields();
            await breedingEventStore.Create(record);
            modalNavigationStore.Close();
        }

        private void ExecuteCancelCommand()
        {
            modalNavigationStore.Close();
        }

        private BreedingEvent GetBreedingEventFromFields()
        {
            return new BreedingEvent(1, _animalId, dateConverterService.GetDateOnly(_aiDate), _expectedFarrowDate);
        }

        private void CalculateExpectedFarDate()
        {

            DateTime result = AiDate.AddDays(GestationPeriod);
            ExpectedFarrowDate = dateConverterService.GetDateOnly(result);
        }

        #endregion
    }
}
