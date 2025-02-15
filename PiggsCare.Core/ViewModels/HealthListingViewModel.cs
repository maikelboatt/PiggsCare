using MvvmCross.ViewModels;

namespace PiggsCare.Core.ViewModels
{
    public class HealthListingViewModel:MvxViewModel, IHealthListingViewModel
    {
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

        #region Fields

        private DateTime _recordDate;
        private string _diagnosis;
        private string _treatment;
        private string _outcome;

        #endregion
    }
}
