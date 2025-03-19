using PiggsCare.Domain.Models;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PiggsCare.Core.Validation
{
    public class WeaningRecordValidation:IWeaningRecordValidation, INotifyDataErrorInfo, INotifyPropertyChanged
    {
        #region Fields

        // Dictionary to hold errors with property names as keys
        public Dictionary<string, List<string>> Errors { get; set; } = new();

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


        #region Event Handler

        private void RaiseErrorsChanged( string propertyName )
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            RaisePropertyChanged(nameof(HasErrors));
        }


        protected virtual void RaisePropertyChanged( [CallerMemberName] string? propertyName = null )
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>( ref T field, T value, [CallerMemberName] string? propertyName = null )
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        #endregion

        #region Events

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

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
                case nameof(WeaningEvent.WeaningDate):
                    ValidateWeaningDate((DateOnly)value, propertyName);
                    break;
                case nameof(WeaningEvent.NumberWeaned):
                case nameof(WeaningEvent.MalesWeaned):
                case nameof(WeaningEvent.FemalesWeaned):
                    ValidateIntProperty((int)value, propertyName);
                    break;
                case nameof(WeaningEvent.AverageWeaningWeight):
                    ValidateFloatProperty((float)value, propertyName);
                    break;
            }
        }

        private void ValidateWeaningDate( DateOnly value, string propertyName )
        {
            ClearError(propertyName);
            if (value == default)
                AddError(propertyName, "Record date is required.");
            else if (value > DateOnly.FromDateTime(DateTime.Now))
                AddError(propertyName, "Record date cannot be greater than the current date.");
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
    }
}
