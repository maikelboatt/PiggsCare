using PiggsCare.Core.ViewModels;

namespace PiggsCare.Core.Commands.NavigationCommands
{
    /// <summary>
    ///     Command to switch the Main Section of the MainViewModel to the Analytics View
    /// </summary>
    /// <param name="mainViewModel" >The MainViewModel</param>
    public class NavigateToAnalyticsCommand( IMainViewModel mainViewModel ):CommandBase
    {
        public override void Execute( object? parameter )
        {
            mainViewModel.CurrentViewModel = new AnalyticsViewModel();
        }
    }
}
