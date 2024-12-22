using PiggsCare.Core.ViewModels;

namespace PiggsCare.Core.Commands.NavigationCommands
{
    /// <summary>
    ///     Command to switch the Main Section of the MainViewModel to the Notification View
    /// </summary>
    /// <param name="mainViewModel" >The MainViewModel</param>
    public class NavigateToNotificationCommand( IMainViewModel mainViewModel ):CommandBase
    {
        public override void Execute( object? parameter )
        {
            mainViewModel.CurrentViewModel = new NotificationViewModel();
        }
    }
}
