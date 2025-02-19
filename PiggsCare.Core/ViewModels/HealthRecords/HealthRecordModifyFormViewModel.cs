using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;

namespace PiggsCare.Core.ViewModels.HealthRecords
{
    public class HealthRecordModifyFormViewModel( IHealthRecordStore healthRecordStore, ModalNavigationStore modalNavigationStore ):MvxViewModel<int>, IHealthRecordModifyFormViewModel
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

        private DateTime _recordDate;
        private string _diagnosis;
        private string _treatment;
        private string _outcome;
        private int _healthRecordId;
        private int _animalId;

        #endregion

        #region Properties

        public DateTime RecordDate
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

        private async Task ExecuteSubmitRecord()
        {
            HealthRecord record = GetHealthRecordFromFields();
            await healthRecordStore.Modify(record);
            modalNavigationStore.Close();
        }

        private HealthRecord GetHealthRecordFromFields()
        {
            return new HealthRecord(_healthRecordId, _animalId, DateTime.Today, _diagnosis, _treatment, _outcome);
            // Todo: use inserted date in both create instead of the DateTime object
        }

        private void ExecuteCancelCommand()
        {
            modalNavigationStore.Close();
        }

        #endregion
    }
}
