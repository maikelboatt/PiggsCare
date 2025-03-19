using System.Windows;

namespace PiggsCare.Domain.Services
{
    public interface IMessageService
    {
        MessageBoxResult Show( string message, string caption, MessageBoxButton buttons, MessageBoxImage image );
    }
}
