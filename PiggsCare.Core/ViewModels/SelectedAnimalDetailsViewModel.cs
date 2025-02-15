using MvvmCross.ViewModels;
using PiggsCare.Core.Factory;

namespace PiggsCare.Core.ViewModels
{
    public class SelectedAnimalDetailsViewModel( IViewModelFactory viewModelFactory ):MvxViewModel<int>, ISelectedAnimalsDetailsViewModel
    {
        public override void Prepare( int parameter )
        {
            Console.WriteLine(parameter);
            SetupViewModel();
        }


        private void SetupViewModel()
        {
            HealthListingViewModel? viewmodel = viewModelFactory.CreateViewModel<HealthListingViewModel>();
            CurrentViewModel = viewmodel;
            viewmodel?.Initialize();
        }

        #region Fields

        private int _animalId;
        private int _name;
        private string _breed;
        private DateTime _birthDate;
        private int _certificateNumber;
        private string _gender;
        private float _backFatIndex;
        private MvxViewModel? _currentViewModel;

        #endregion

        #region Properties

        public MvxViewModel? CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public int Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public string Breed
        {
            get => _breed;
            set => SetProperty(ref _breed, value);
        }
        public DateTime BirthDate
        {
            get => _birthDate;
            set => SetProperty(ref _birthDate, value);
        }
        public int CertificateNumber
        {
            get => _certificateNumber;
            set => SetProperty(ref _certificateNumber, value);
        }
        public string Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }
        public float BackFatIndex
        {
            get => _backFatIndex;
            set => SetProperty(ref _backFatIndex, value);
        }

        #endregion
    }
}
