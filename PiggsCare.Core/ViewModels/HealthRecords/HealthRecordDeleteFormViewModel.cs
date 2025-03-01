using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using System.Windows;

namespace PiggsCare.Core.ViewModels.HealthRecords
{
    public class HealthRecordDeleteFormViewModel( IHealthRecordStore healthRecordStore, ModalNavigationStore modalNavigationStore ):MvxViewModel<int>, IHealthRecordDeleteFormViewModel
    {
        public override void Prepare( int parameter )
        {
            _healthRecordId = parameter;
        }

        public override Task Initialize()
        {
            HealthRecord? record = healthRecordStore?.HealthRecords.FirstOrDefault(x => x.HealthRecordId == _healthRecordId);
            if (record == null) return base.Initialize();
            PopulateEditForm(record);
            _animalId = record.AnimalId;
            return base.Initialize();
        }

        private void PopulateEditForm( HealthRecord record )
        {
            _recordDate = record.RecordDate;
            _diagnosis = record.Diagnosis;
            _treatment = record.Treatment;
            _outcome = record.Outcome;
        }


        #region Fields

        private DateOnly _recordDate;
        private string _diagnosis;
        private string _treatment;
        private string _outcome;
        private int _healthRecordId;
        private int _animalId;

        #endregion

        #region Properties

        public DateOnly RecordDate
        {
            get => _recordDate;
            set => SetProperty(ref _recordDate, value);
        }
        public string Diagnosis
        {
            get => _diagnosis;
            set => SetProperty(ref _diagnosis, value);
        }
        public string Treatment
        {
            get => _treatment;
            set => SetProperty(ref _treatment, value);
        }
        public string Outcome
        {
            get => _outcome;
            set => SetProperty(ref _outcome, value);
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
            await healthRecordStore.Remove(_healthRecordId);
            modalNavigationStore.Close();
        }

        #endregion
    }
}
