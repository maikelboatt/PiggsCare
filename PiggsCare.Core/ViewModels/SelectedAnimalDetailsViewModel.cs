using MvvmCross.ViewModels;

namespace PiggsCare.Core.ViewModels
{
    public class SelectedAnimalDetailsViewModel:MvxViewModel<int>, ISelectedAnimalsDetailsViewModel
    {
        public override void Prepare( int parameter )
        {
            Console.WriteLine(parameter);
        }

        #region Fields

        private int _animalId;
        private int _name;
        private string _breed;
        private DateTime _birthDate;
        private int _certificateNumber;
        private string _gender;
        private float _backFatIndex;

        #endregion

        #region Properties

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
