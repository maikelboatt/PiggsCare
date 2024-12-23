using PiggsCare.Core.ViewModels;

namespace PiggsCare.Core.Commands
{
    /// <summary>
    ///     Command for switching between views in the MainView.xaml
    /// </summary>
    /// <param name="viewModelBase" >The viewmodel of the view t switch to</param>
    /// <param name="mainViewModel" >An interface of the MainViewModel for accessing the current main section viewmodel</param>
    public class ChangeMainSectionCommand( ViewModelBase viewModelBase, IMainViewModel mainViewModel ):CommandBase
    {
        public override void Execute( object? parameter )
        {
            mainViewModel.CurrentViewModel = viewModelBase;
        }
    }
}
