using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;
using System.Windows;

namespace PiggsCare.Core.ViewModels.Pregnancy
{
    public class PregnancyDeleteFormViewModel(
        IPregnancyStore pregnancyStore,
        ModalNavigationStore modalNavigationStore,
        IDateConverterService dateConverterService,
        IMessageService messageService )
        :MvxViewModel<int>, IPregnancyDeleteFormViewModel
    {
        #region ViewModel Life-Cycle

        public override void Prepare( int parameter )
        {
            _pregnancyScanId = parameter;
        }

        public override Task Initialize()
        {
            PregnancyScan? record = pregnancyStore?.PregnancyScans.FirstOrDefault(x => x.ScanId == _pregnancyScanId);
            if (record == null) return base.Initialize();
            PopulateDeleteForm(record);
            _breedingEventId = record.BreedingEventId;
            return base.Initialize();
        }

        #endregion

        #region Fields

        private int _breedingEventId;
        private int _pregnancyScanId;
        private DateTime _scanDate;
        private string _scanResults;

        #endregion

        #region Properties

        public DateTime ScanDate
        {
            get => _scanDate;
            set
            {
                if (value.Equals(_scanDate)) return;
                _scanDate = value;
                RaisePropertyChanged(() => ScanDate);
            }
        }
        public string ScanResults
        {
            get => _scanResults;
            set
            {
                if (value == _scanResults) return;
                _scanResults = value;
                RaisePropertyChanged(() => ScanResults);
            }
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand SubmitRecordCommand => new MvxAsyncCommand(ExecuteSubmitRecord);

        public IMvxCommand CancelRecordCommand => new MvxCommand(ExecuteCancelCommand);

        #endregion

        #region Methods

        private void PopulateDeleteForm( PregnancyScan scan )
        {
            _scanDate = dateConverterService.GetDateTime(scan.ScanDate);
            _scanResults = scan.ScanResults;
        }

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
            await pregnancyStore.Remove(_pregnancyScanId);
            modalNavigationStore.Close();
        }

        #endregion
    }
}
