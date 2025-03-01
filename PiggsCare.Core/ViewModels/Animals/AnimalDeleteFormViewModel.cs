using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using System.Windows;

namespace PiggsCare.Core.ViewModels.Animals
{
    public class AnimalDeleteFormViewModel( ModalNavigationStore modalNavigationStore, IAnimalStore animalStore ):MvxViewModel<int>, IAnimalDeleteFormViewModel
    {
        #region Methods

        private void PopulateEditForm( Animal animal )
        {
            _name = animal.Name;
            _breed = animal.Breed;
            _birthDate = animal.BirthDate;
            _certificateNumber = animal.CertificateNumber;
            _gender = animal.Gender;
            _backFatIndex = animal.BackFatIndex;
        }

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

            await animalStore.Remove(_animalId);
            modalNavigationStore.Close();
        }

        #endregion

        #region ViewModelLifeCycle

        public override void Prepare( int parameter )
        {
            _animalId = parameter;
        }

        public override Task Initialize()
        {
            Animal? record = animalStore.Animals.FirstOrDefault(x => x.AnimalId == _animalId);
            if (record != null)
                PopulateEditForm(record);
            return base.Initialize();
        }

        #endregion

        #region Fields

        private int _animalId;
        private int _name;
        private string _breed;
        private DateOnly _birthDate;
        private int _certificateNumber;
        private string _gender;
        private float _backFatIndex;

        #endregion

        #region Commands

        public IMvxAsyncCommand SubmitRecordCommand => new MvxAsyncCommand(ExecuteSubmitRecord);
        public IMvxCommand CancelRecordCommand => new MvxCommand(ExecuteCancelCommand);

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
        public DateOnly BirthDate
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
