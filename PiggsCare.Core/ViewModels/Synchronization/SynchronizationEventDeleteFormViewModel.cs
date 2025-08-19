using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores;
using PiggsCare.ApplicationState.Stores.Insemination;
using PiggsCare.Business.Services.Insemination;
using PiggsCare.Business.Services.Message;
using PiggsCare.Business.Services.Synchronization;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Services;
using System.Windows;

namespace PiggsCare.Core.ViewModels.Synchronization
{
    public class SynchronizationEventDeleteFormViewModel(
        ISynchronizationService synchronizationService,
        IInseminationService inseminationService,
        IInseminationEventStore inseminationEventStore,
        ModalNavigationStore modalNavigationStore,
        IDateConverterService dateConverterService,
        IMessageService messageService )
        :MvxViewModel<int>, ISynchronizationEventDeleteFormViewModel
    {
        #region ViewModel Life-Cycle

        public override void Prepare( int parameter )
        {
            _synchronizationId = parameter;
        }

        public override Task Initialize()
        {
            SynchronizationEvent? record = synchronizationService.GetSynchronizationEventByIdAsync(_synchronizationId);
            if (record is null) return base.Initialize();
            PopulateDeleteForm(record);
            return base.Initialize();
        }

        #endregion

        #region Fields

        private DateTime _startDate;
        private DateOnly _endDate;
        private string _batchNumber = string.Empty;
        private string _synchronizationProtocol = string.Empty;
        private string _comments = string.Empty;
        private int _synchronizationId;

        private const int SynchronizationPeriodInDays = 18;

        #endregion

        #region Properties

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (value.Equals(_startDate)) return;
                _startDate = value;
                RaisePropertyChanged();
            }
        }

        public DateOnly EndDate
        {
            get => _endDate;
            set
            {
                if (value.Equals(_endDate)) return;
                _endDate = value;
                RaisePropertyChanged();
            }
        }

        public string SynchronizationProtocol
        {
            get => _synchronizationProtocol;
            set
            {
                if (value.Equals(_synchronizationProtocol)) return;
                _synchronizationProtocol = value;
                RaisePropertyChanged();
            }
        }
        public string BatchNumber
        {
            get => _batchNumber;
            set
            {
                if (value.Equals(_batchNumber)) return;
                _batchNumber = value;
                RaisePropertyChanged();
            }
        }
        public string Comments
        {
            get => _comments;
            set
            {
                if (value.Equals(_comments)) return;
                _comments = value;
            }
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand SubmitRecordCommand => new MvxAsyncCommand(ExecuteSubmitRecord);

        public IMvxCommand CancelRecordCommand => new MvxCommand(ExecuteCancelCommand);

        #endregion

        #region Methods

        private async Task ExecuteSubmitRecord()
        {
            MessageBoxResult confirm = messageService.Show("Are you sure you want to delete the record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.OK)
                await OnDeleteConfirm();
        }

        private async Task OnDeleteConfirm()
        {
            await DeleteCorrespondingBreedingEvent();
            await synchronizationService.DeleteSynchronizationEventAsync(_synchronizationId);
            modalNavigationStore.Close();
        }

        private async Task DeleteCorrespondingBreedingEvent()
        {
            inseminationService.GetAllInseminationEventBySynchronizationBatchAsync(_synchronizationId);

            foreach (InseminationEventWithAnimal record in inseminationEventStore.InseminationEventsWithAnimals)
                await inseminationService.DeleteInseminationEventAsync(record.BreedingEventId);
        }


        private void ExecuteCancelCommand()
        {
            modalNavigationStore.Close();
        }

        private void PopulateDeleteForm( SynchronizationEvent synchronizationEvent )
        {
            _synchronizationId = synchronizationEvent.SynchronizationEventId;
            _startDate = dateConverterService.GetDateTime(synchronizationEvent.StartDate);
            _endDate = synchronizationEvent.EndDate;
            _batchNumber = synchronizationEvent.BatchNumber;
            _synchronizationProtocol = synchronizationEvent.SynchronizationProtocol;
            _comments = synchronizationEvent.Comments;
        }

        #endregion
    }
}
