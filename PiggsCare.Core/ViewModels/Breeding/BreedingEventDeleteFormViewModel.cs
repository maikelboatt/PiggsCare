using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores;
using PiggsCare.ApplicationState.Stores.Insemination;
using PiggsCare.Business.Services.Insemination;
using PiggsCare.Business.Services.Message;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Services;
using System.Windows;

namespace PiggsCare.Core.ViewModels.Breeding
{
    public class BreedingEventDeleteFormViewModel(
        IInseminationEventStore inseminationEventStore,
        IInseminationService inseminationService,
        ModalNavigationStore modalNavigationStore,
        IDateConverterService dateConverterService,
        IMessageService messageService )
        :MvxViewModel<int>, IBreedingEventDeleteFormViewModel
    {
        public override Task Initialize()
        {
            InseminationEvent? record = inseminationService.GetInseminationEventByIdAsync(_breedingEventId);
            if (record == null) return base.Initialize();
            PopulateDeleteForm(record);
            _animalId = record.AnimalId;
            return base.Initialize();
        }

        public override void Prepare( int parameter )
        {
            _breedingEventId = parameter;
        }

        private void PopulateDeleteForm( InseminationEvent inseminationEvent )
        {
            _breedingEventId = inseminationEvent.BreedingEventId;
            _animalId = inseminationEvent.AnimalId;
            _aiDate = dateConverterService.GetDateTime(inseminationEvent.AiDate);
            _expectedFarrowDate = inseminationEvent.ExpectedFarrowDate;
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
            MessageBoxResult confirm = messageService.Show("Are you sure you want to delete the record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.OK)
                await OnDeleteConfirm();
        }

        private async Task OnDeleteConfirm()
        {
            await inseminationService.DeleteInseminationEventAsync(_breedingEventId);
            modalNavigationStore.Close();
        }

        #endregion
    }
}
