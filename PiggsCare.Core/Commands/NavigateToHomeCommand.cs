using PiggsCare.Core.ViewModels;

namespace PiggsCare.Core.Commands
{
    public class NavigateToHomeCommand( MainViewModel mainViewModel ):CommandBase
    {
        public override void Execute( object? parameter )
        {
            mainViewModel.CurrentViewModel = new HomeViewModel();
        }
    }
}
