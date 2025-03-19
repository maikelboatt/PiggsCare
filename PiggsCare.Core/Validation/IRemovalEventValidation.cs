using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PiggsCare.Core.Validation
{
    public interface IRemovalEventValidation
    {
        bool HasErrors { get; }

        Dictionary<string, List<string>> Errors { get; set; }

        IEnumerable GetErrors( string? propertyName );

        event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        void ValidateProp( object value, [CallerMemberName] string propertyName = "" );
    }
}
