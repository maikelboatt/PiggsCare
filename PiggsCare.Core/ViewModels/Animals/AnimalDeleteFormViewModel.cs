using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores;
using PiggsCare.Business.Services.Animals;
using PiggsCare.Business.Services.Message;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Services;
using System.Windows;

namespace PiggsCare.Core.ViewModels.Animals
{
    public class AnimalDeleteFormViewModel(
        ModalNavigationStore modalNavigationStore,
        IAnimalService animalService,
        IMessageService messageService,
        IDateConverterService dateConverterService )
        :MvxViewModel<int>, IAnimalDeleteFormViewModel
    {
        #region Methods

        private void PopulateEditForm( Animal animal )
        {
            _name = animal.Name;
            _breed = animal.Breed;
            _birthDate = dateConverterService.GetDateTime(animal.BirthDate);
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
            MessageBoxResult confirm = messageService.Show("Are you sure you want to delete the record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.OK)
                await OnDeleteConfirm();
        }

        private async Task OnDeleteConfirm()
        {

            await animalService.DeleteAnimalAsync(_animalId);
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
            Animal? record = animalService.GetAnimalById(_animalId);
            if (record != null)
                PopulateEditForm(record);
            return base.Initialize();
        }

        #endregion

        #region Fields

        private int _animalId;
        private int _name;
        private string _breed = string.Empty;
        private DateTime _birthDate;
        private int _certificateNumber;
        private string _gender = string.Empty;
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
