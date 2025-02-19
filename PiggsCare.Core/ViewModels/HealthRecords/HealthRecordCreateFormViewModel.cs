using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;

namespace PiggsCare.Core.ViewModels.HealthRecords
{
    public class HealthRecordCreateFormViewModel( IHealthRecordStore healthRecordStore, ModalNavigationStore modalNavigationStore ):MvxViewModel<int>, IHealthRecordCreateFormViewModel
    {
        #region ViewModel Life-Cycle

        public override void Prepare( int parameter )
        {
            _animalId = parameter;
        }

        #endregion


        #region Fields

        private DateTime _recordDate;
        private string _diagnosis;
        private string _treatment;
        private string _outcome;
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
            await healthRecordStore.Create(record);
            modalNavigationStore.Close();
        }


        private void ExecuteCancelCommand()
        {
            modalNavigationStore.Close();
        }

        private HealthRecord GetHealthRecordFromFields()
        {
            return new HealthRecord(1, _animalId, DateTime.Today, _diagnosis, _treatment, _outcome);
            // Todo: use inserted date in both create instead of the DateTime object
        }

        #endregion
    }
}
