using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;
using System.Windows;

namespace PiggsCare.Core.ViewModels.Breeding
{
    public class BreedingEventDeleteFormViewModel( IBreedingEventStore breedingEventStore, ModalNavigationStore modalNavigationStore, IDateConverterService dateConverterService )
        :MvxViewModel<int>, IBreedingEventDeleteFormViewModel
    {
        public override Task Initialize()
        {
            BreedingEvent? record = breedingEventStore?.BreedingEvents.FirstOrDefault(x => x.BreedingEventId == _breedingEventId);
            if (record == null) return base.Initialize();
            PopulateDeleteForm(record);
            _animalId = record.AnimalId;
            return base.Initialize();
        }

        public override void Prepare( int parameter )
        {
            _breedingEventId = parameter;
        }

        private void PopulateDeleteForm( BreedingEvent breedingEvent )
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
            set => SetProperty(ref _aiDate, value);
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

        private void ExecuteCancelCommand()
        {
            modalNavigationStore.Close();
        }

        private async Task ExecuteSubmitRecord()
        {
            MessageBoxResult confirm = MessageBox.Show("Are you sure you want to delete the record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.OK)
                await OnDeleteConfirm();
        }

        private async Task OnDeleteConfirm()
        {
            await breedingEventStore.Remove(_breedingEventId);
            modalNavigationStore.Close();
        }

        #endregion
    }
}
