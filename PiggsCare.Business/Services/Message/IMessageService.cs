using System.Windows;

namespace PiggsCare.Business.Services.Message
{
    public interface IMessageService
    {
        MessageBoxResult Show( string message, string caption, MessageBoxButton button, MessageBoxImage icon );
    }
}
