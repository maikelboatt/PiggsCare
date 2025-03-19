using System.Windows;

namespace PiggsCare.Domain.Services
{
    public class MessageService:IMessageService
    {
        public MessageBoxResult Show( string message, string caption, MessageBoxButton buttons, MessageBoxImage image )
        {
            return MessageBox.Show(message, caption, buttons);
        }
    }
}
