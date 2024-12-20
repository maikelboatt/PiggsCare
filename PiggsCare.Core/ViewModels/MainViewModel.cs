using PiggsCare.Core.Commands;
using System.Windows.Input;

namespace PiggsCare.Core.ViewModels
{
    public class MainViewModel:ViewModelBase
    {
        private ViewModelBase _currentViewModel = new AnalyticsViewModel();
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetField(ref _currentViewModel, value);
        }

        #region Commands

        public ICommand NavigateToHome { get; private set; }

        public MainViewModel()
        {
            NavigateToHome = new NavigateToHomeCommand(this);
        }

        #endregion
    }
}
