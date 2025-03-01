using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Core.Validation;
using PiggsCare.Domain.Models;
using System.Collections;
using System.ComponentModel;

namespace PiggsCare.Core.ViewModels.Animals
{
    public class AnimalCreateFormViewModel:MvxViewModel<int>, IAnimalCreateFormViewModel, INotifyDataErrorInfo
    {
        #region ViewModel Life-Cycle

        public override void Prepare( int parameter )
        {
            Console.WriteLine(parameter);
        }

        #endregion

        #region Constructor

        public AnimalCreateFormViewModel( ModalNavigationStore modalNavigationStore, IAnimalStore animalStore, IAnimalRecordValidation recordValidation )
        {
            _modalNavigationStore = modalNavigationStore;
            _animalStore = animalStore;
            _recordValidation = recordValidation;
            _recordValidation.Errors.Clear();

            recordValidation.ErrorsChanged += RecordValidationOnErrorsChanged;
        }

        private void RecordValidationOnErrorsChanged( object? sender, DataErrorsChangedEventArgs e )
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(e.PropertyName));
            RaisePropertyChanged(nameof(HasErrors));
            RaisePropertyChanged(nameof(CanSubmitRecord));
        }

        #endregion


        #region Methods

        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        private async Task ExecuteSubmitRecord()
        {
            Animal record = GetAnimalFromFields();
            await _animalStore.Create(record);
            _modalNavigationStore.Close();
        }

        private Animal GetAnimalFromFields()
        {
            return new Animal(1, Name, Breed, BirthDate, CertificateNumber, Gender, BackFatIndex);
        }

        #endregion

        #region Properties

        public int Name
        {
            get => _name;
            set
            {
                if (value.Equals(_name)) return;
                _name = value;
                _recordValidation.ValidateProp(value);
                RaisePropertyChanged();
            }
        }

        public string Breed
        {
            get => _breed;
            set
            {
                if (value.Equals(_breed)) return;
                _breed = value;
                _recordValidation.ValidateProp(value);
                RaisePropertyChanged();
            }
        }

        public DateOnly BirthDate
        {
            get => _birthDate;
            set
            {
                if (value.Equals(_birthDate)) return;
                _birthDate = value;
                _recordValidation.ValidateProp(value);
                RaisePropertyChanged();
            }
        }

        public int CertificateNumber
        {
            get => _certificateNumber;
            set
            {
                if (value.Equals(_certificateNumber)) return;
                _certificateNumber = value;
                _recordValidation.ValidateProp(value);
                RaisePropertyChanged();
            }
        }

        public string Gender
        {
            get => _gender;
            set
            {
                if (value.Equals(_gender)) return;
                _gender = value;
                _recordValidation.ValidateProp(value);
                RaisePropertyChanged();
            }
        }

        public float BackFatIndex
        {
            get => _backFatIndex;
            set
            {
                if (value.Equals(_backFatIndex)) return;
                _backFatIndex = value;
                _recordValidation.ValidateProp(value);
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand SubmitRecordCommand => new MvxAsyncCommand(ExecuteSubmitRecord, CanSubmitRecord);

        private bool CanSubmitRecord()
        {
            return !HasErrors;
        }

        public IMvxCommand CancelRecordCommand => new MvxCommand(ExecuteCancelCommand);

        #endregion


        #region Fields

        private int _name;
        private string _breed = string.Empty;
        private DateOnly _birthDate;
        private int _certificateNumber;
        private string _gender = string.Empty;
        private float _backFatIndex;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly IAnimalStore _animalStore;
        private readonly IAnimalRecordValidation _recordValidation;

        #endregion

        #region INotifyDataErrorInfo Implementation

        public bool HasErrors => _recordValidation.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable? GetErrors( string? propertyName )
        {
            return _recordValidation.GetErrors(propertyName);
        }

        #endregion
    }
}
