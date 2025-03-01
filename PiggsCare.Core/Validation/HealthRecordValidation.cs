using PiggsCare.Domain.Models;
using System.Collections;
using System.ComponentModel;

namespace PiggsCare.Core.Validation
{
    public class HealthRecordValidation:INotifyDataErrorInfo, IHealthRecordValidation
    {
        #region Fields

        public Dictionary<string, List<string>> Errors { get; set; } = new();

        #endregion

        #region Events

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        #endregion


        public IEnumerable? GetErrors( string? propertyName )
        {
            if (string.IsNullOrEmpty(propertyName))
                return null;

            return Errors.ContainsKey(propertyName) ? Errors[propertyName] : null;
        }

        #region Props

        public bool HasErrors => Errors.Count > 0;

        #endregion

        #region Event Handler

        private void RaiseErrorsChanged( string propertyName )
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
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

        #region Validation Methods

        public void ValidateProp( object value, string propertyName = "" )
        {
            switch (propertyName)
            {
                case nameof(HealthRecord.RecordDate):
                    ValidateRecordDate((DateOnly)value, propertyName);
                    break;
                case nameof(HealthRecord.Diagnosis):
                case nameof(HealthRecord.Treatment):
                case nameof(HealthRecord.Outcome):
                    ValidateStringProperty((string)value, propertyName, "Field cannot be empty", 3, 30);
                    break;
            }
        }

        private void ValidateRecordDate( DateOnly value, string propertyName )
        {
            ClearError(propertyName);
            if (value == default)
                AddError(propertyName, "Record date is required.");
            else if (value > DateOnly.FromDateTime(DateTime.Now))
                AddError(propertyName, "Record date cannot be greater than the current date.");
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

        #endregion
    }
}
