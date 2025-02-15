using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PiggsCare.Core.ViewModels
{
    public class AnimalCreateFormViewModel( ModalNavigationStore modalNavigationStore, IAnimalStore animalStore ):MvxViewModel<int>, IAnimalCreateFormViewModel, INotifyDataErrorInfo
    {
        public override void Prepare( int parameter )
        {
            Console.WriteLine(parameter);
        }

        #region Validation

        private void ValidateProperty<T>( T value, string propertyName )
        {
            ValidationContext validationContext = new(this) { MemberName = propertyName };
            List<ValidationResult> results = [];

            if (Validator.TryValidateProperty(value, validationContext, results))
            {
                ClearErrors(propertyName);
            }
            else
            {
                AddErrors(propertyName, results.Select(r => r.ErrorMessage));
            }
        }

        #endregion

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

        [Required(ErrorMessage = "Please enter the name of the animal")]
        public int Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        [Required(ErrorMessage = "Please enter the breed of the animal")]
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
        [Required(ErrorMessage = "Please enter the certificate number of the animal")]
        public int CertificateNumber
        {
            get => _certificateNumber;
            set => SetProperty(ref _certificateNumber, value);
        }
        [Required(ErrorMessage = "Please enter the gender of the animal")]
        public string Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }
        [Required(ErrorMessage = "Please enter the back-fat index of the animal")]
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

        #region INotifyDataErrorInfo Implementation

        public Dictionary<string, List<string>> Errors { get; } = new();
        public bool HasErrors => Errors.Count != 0;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors( string? propertyName )
        {
            if (string.IsNullOrEmpty(propertyName) || !Errors.ContainsKey(propertyName))
            {
                return new List<string>(); // Return empty list if no errors
            }

            return Errors[propertyName]; // Return existing errors
        }

        private void AddErrors( string propertyName, IEnumerable<string?> errorMessages )
        {
            if (!Errors.ContainsKey(propertyName))
            {
                Errors[propertyName] = new List<string>();
            }

            Errors[propertyName].AddRange(errorMessages); // Add the new errors

            OnErrorsChanged(propertyName); // Raise event to notify UI
        }

        private void ClearErrors( string propertyName )
        {
            if (!Errors.ContainsKey(propertyName)) return;
            Errors.Remove(propertyName);   // Remove errors for the property
            OnErrorsChanged(propertyName); // Raise event to notify UI
        }

        protected virtual void OnErrorsChanged( string propertyName )
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        #endregion
    }
}
