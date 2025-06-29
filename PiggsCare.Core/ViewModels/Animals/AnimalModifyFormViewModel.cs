using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Core.Validation;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;
using System.Collections;
using System.ComponentModel;

namespace PiggsCare.Core.ViewModels.Animals
{
    public class AnimalModifyFormViewModel:MvxViewModel<int>, IAnimalModifyFormViewModel, INotifyDataErrorInfo
    {
        #region Constructor

        public AnimalModifyFormViewModel( ModalNavigationStore modalNavigationStore, IAnimalStore animalStore, IAnimalRecordValidation recordValidation,
            IDateConverterService dateConverterService )
        {
            _modalNavigationStore = modalNavigationStore;
            _animalStore = animalStore;
            _recordValidation = recordValidation;
            _dateConverterService = dateConverterService;
            _recordValidation.Errors.Clear();

            recordValidation.ErrorsChanged += RecordValidationOnErrorsChanged;

            // Initialize commands once so that RaiseCanExecuteChanged works as expected
            SubmitRecordCommand = new MvxAsyncCommand(ExecuteSubmitRecord, CanSubmitRecord);
            CancelRecordCommand = new MvxCommand(ExecuteCancelCommand);
        }

        private void RecordValidationOnErrorsChanged( object? sender, DataErrorsChangedEventArgs e )
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(e.PropertyName));
            RaisePropertyChanged(nameof(HasErrors));
            RaisePropertyChanged(nameof(CanSubmitRecord));
        }

        #endregion

        #region ViewModel Life-Cycle

        public override Task Initialize()
        {
            Animal? record = _animalStore.Animals?.FirstOrDefault(animal => animal.AnimalId == _animalId);
            if (record is not null)
                PopulateEditForm(record);
            return base.Initialize();
        }

        public override void Prepare( int parameter )
        {
            _animalId = parameter;
        }

        public override void ViewDestroy( bool viewFinishing = true )
        {
            base.ViewDestroy(viewFinishing);
            _recordValidation.ErrorsChanged -= RecordValidationOnErrorsChanged;
        }

        #endregion

        #region Methods

        private void PopulateEditForm( Animal animal )
        {
            _name = animal.Name;
            _breed = animal.Breed;
            _birthDate = _dateConverterService.GetDateTime(animal.BirthDate);
            _certificateNumber = animal.CertificateNumber;
            _gender = animal.Gender;
            _backFatIndex = animal.BackFatIndex;
        }

        private void ExecuteCancelCommand()
        {
            _modalNavigationStore.Close();
        }

        private async Task ExecuteSubmitRecord()
        {
            Animal record = GetAnimalFromFields();
            await _animalStore.Modify(record);
            _modalNavigationStore.Close();
        }

        private Animal GetAnimalFromFields()
        {
            return new Animal(_animalId, Name, Breed, _dateConverterService.GetDateOnly(BirthDate), CertificateNumber, Gender, BackFatIndex);
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
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly IAnimalStore _animalStore;
        private readonly IAnimalRecordValidation _recordValidation;
        private readonly IDateConverterService _dateConverterService;

        #endregion

        #region Commands

        public IMvxAsyncCommand SubmitRecordCommand { get; }

        private bool CanSubmitRecord()
        {
            bool noFieldEmpty = !string.IsNullOrWhiteSpace(Breed) &&
                                !string.IsNullOrWhiteSpace(Gender) &&
                                // !BackFatIndex.Equals(default) &&
                                // !CertificateNumber.Equals(default) &&
                                // !Name.Equals(default) &&
                                !BirthDate.Equals(default);
            return noFieldEmpty && !HasErrors;
        }

        public IMvxCommand CancelRecordCommand { get; }

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
                SubmitRecordCommand.RaiseCanExecuteChanged();
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
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }
        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (value.Equals(_birthDate)) return;
                _birthDate = value;
                _recordValidation.ValidateProp(_dateConverterService.GetDateOnly(_birthDate));
                RaisePropertyChanged();
                SubmitRecordCommand.RaiseCanExecuteChanged();
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
                SubmitRecordCommand.RaiseCanExecuteChanged();
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
                SubmitRecordCommand.RaiseCanExecuteChanged();
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
                SubmitRecordCommand.RaiseCanExecuteChanged();
            }
        }

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
