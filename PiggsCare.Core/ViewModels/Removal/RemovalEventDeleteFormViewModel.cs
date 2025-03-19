using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;
using System.Windows;

namespace PiggsCare.Core.ViewModels.Removal
{
    public class RemovalEventDeleteFormViewModel(
        IRemovalEventStore eventStore,
        ModalNavigationStore modalNavigationStore,
        IDateConverterService dateConverterService,
        IMessageService messageService )
        :MvxViewModel<int>, IRemovalEventDeleteFormViewModel
    {
        #region ViewModel Life-Cycle

        public override void Prepare( int parameter )
        {
            _removalId = parameter;
        }

        public override Task Initialize()
        {
            RemovalEvent? record = eventStore?.RemovalEvents.FirstOrDefault(x => x.RemovalEventId == _removalId);
            if (record is null) return base.Initialize();
            PopulateDeleteForm(record);
            _animalId = record.AnimalId;
            return base.Initialize();
        }

        #endregion

        #region Fields

        private int _removalId;
        private int _animalId;
        private DateTime _removalDate;
        private string _reasonForRemoval = string.Empty;

        #endregion

        #region Properties

        public DateTime RemovalDate
        {
            get => _removalDate;
            set
            {
                if (value.Equals(_removalDate)) return;
                _removalDate = value;
                RaisePropertyChanged();
            }
        }
        public string ReasonForRemoval
        {
            get => _reasonForRemoval;
            set
            {
                if (value == _reasonForRemoval) return;
                _reasonForRemoval = value ?? throw new ArgumentNullException(nameof(value));
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand SubmitRecordCommand => new MvxAsyncCommand(ExecuteSubmitRecord);

        public IMvxCommand CancelRecordCommand => new MvxCommand(ExecuteCancelCommand);

        #endregion


        #region Methods

        private void PopulateDeleteForm( RemovalEvent record )
        {
            _removalId = record.RemovalEventId;
            _animalId = record.AnimalId;
            _removalDate = dateConverterService.GetDateTime(record.RemovalDate);
            _reasonForRemoval = record.ReasonForRemoval;
        }

        private async Task ExecuteSubmitRecord()
        {
            MessageBoxResult confirm = messageService.Show("Are you sure you want to delete the record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.OK)
                await OnDeleteConfirm();
        }

        private async Task OnDeleteConfirm()
        {
            await eventStore.RemoveAsync(_removalId);
            modalNavigationStore.Close();
        }

        private void ExecuteCancelCommand()
        {
            modalNavigationStore.Close();
        }

        #endregion
    }
}
