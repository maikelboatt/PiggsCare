using PiggsCare.Domain.Models;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PiggsCare.Core.Validation
{
    public class AnimalRecordValidation:INotifyDataErrorInfo, IAnimalRecordValidation
    {
        #region Fields

        public Dictionary<string, List<string>?> Errors { get; set; } = new();

        #endregion

        #region Events

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        #endregion

        #region Props

        public bool HasErrors => Errors.Count > 0;

        #endregion

        public IEnumerable? GetErrors( string? propertyName )
        {

            if (string.IsNullOrEmpty(propertyName))
                return null;

            return Errors.ContainsKey(propertyName) ? Errors[propertyName] : null;
        }

        private void RaiseErrorsChanged( string propertyName )
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        #region Validation Methods

        public void ValidateProp( object value, [CallerMemberName] string propertyName = "" )
        {
            switch (propertyName)
            {
                case nameof(Animal.BirthDate):
                    ValidateDateProperty((DateOnly)value, propertyName);
                    break;
                case nameof(Animal.Name):
                case nameof(Animal.CertificateNumber):
                    ValidateIntProperty((int)value, propertyName);
                    break;
                case nameof(Animal.Breed):
                case nameof(Animal.Gender):
                    ValidateStringProperty((string)value, propertyName, "Field cannot be empty", 3, 30);
                    break;
                case nameof(Animal.BackFatIndex):
                    ValidateFloatProperty((float)value, propertyName);
                    break;
            }
        }

        private void ValidateDateProperty( DateOnly value, string propertyName )
        {
            ClearError(propertyName);
            if (value == default)
                AddError(propertyName, $"{propertyName} is required.");
            else if (value > DateOnly.FromDateTime(DateTime.Now))
                AddError(propertyName, $"{propertyName} cannot be greater than the current date.");
        }

        private void ValidateStringProperty( string value, string propertyName, string errorMessage, int minLength, int maxLength )
        {
            ClearError(propertyName);
            if (string.IsNullOrWhiteSpace(value))
            {
                AddError(propertyName, errorMessage);
            }

            else if (value.Length < minLength || value.Length > maxLength)
            {
                AddError(propertyName, $"{propertyName} has to be between {minLength} and {maxLength}.");
            }
        }

        private void ValidateIntProperty( int value, string propertyName = "" )
        {
            ClearError(propertyName);
            if (value < 1)
                AddError(propertyName, $"{propertyName} must be greater than 0.");
        }

        private void ValidateFloatProperty( float value, string propertyName = "" )
        {
            ClearError(propertyName);
            if (value < float.Epsilon)
                AddError(propertyName, $"{propertyName} must be a positive number.");
        }

        #endregion

        #region Error Mutation

        private void AddError( string propertyName, string error )
        {
            if (!Errors.ContainsKey(propertyName))
                Errors[propertyName] = new List<string>();

            Errors[propertyName].Add(error);
            RaiseErrorsChanged(propertyName);
        }

        private void ClearError( string propertyName )
        {
            if (!Errors.ContainsKey(propertyName)) return;
            Errors.Remove(propertyName);
            RaiseErrorsChanged(propertyName);
        }

        #endregion
    }
}
