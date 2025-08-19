using System.Windows;

namespace PiggsCare.Business.Services.Message
{
    public class MessageService:IMessageService
    {
        public MessageBoxResult Show( string message, string caption, MessageBoxButton button, MessageBoxImage icon ) => MessageBox.Show(message, caption, button, icon);
    }
}
