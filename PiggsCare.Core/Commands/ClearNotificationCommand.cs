using PiggsCare.Core.ViewModels;

namespace PiggsCare.Core.Commands
{
    public class ClearNotificationCommand( NotificationViewModel notificationViewModel ):CommandBase
    {
        public override void Execute( object? parameter )
        {
            notificationViewModel.Messages.Clear();
        }
    }
}
