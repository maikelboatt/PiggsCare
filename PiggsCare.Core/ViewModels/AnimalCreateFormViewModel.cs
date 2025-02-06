using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;

namespace PiggsCare.Core.ViewModels
{
    public class AnimalCreateFormViewModel( ModalNavigationStore modalNavigationStore, IAnimalStore animalStore ):MvxViewModel<int>, IAnimalCreateFormViewModel
    {
        public override void Prepare( int parameter )
        {
            Console.WriteLine(parameter);
        }

        #region Methods

        private void ExecuteCancelCommand()
        {
            modalNavigationStore.Close();
        }

        private async Task ExecuteSubmitRecord()
        {
            Animal record = GetAnimalFromFields();
            await animalStore.Create(record);
            modalNavigationStore.Close();
        }

        private Animal GetAnimalFromFields()
        {
            return new Animal(1, Name, Breed, DateTime.Now, CertificateNumber, Gender, BackFatIndex);
        }

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

        #region Commands

        public IMvxAsyncCommand SubmitRecordCommand => new MvxAsyncCommand(ExecuteSubmitRecord);
        public IMvxCommand CancelRecordCommand => new MvxCommand(ExecuteCancelCommand);

        #endregion


        #region Fields

        private int _name;
        private string _breed;
        private DateTime _birthDate;
        private int _certificateNumber;
        private string _gender;
        private float _backFatIndex;

        #endregion
    }
}
