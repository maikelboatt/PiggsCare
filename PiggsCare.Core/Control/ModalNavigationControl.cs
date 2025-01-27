using MvvmCross;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Core.ViewModels;

namespace PiggsCare.Core.Control
{
    public class ModalNavigationControl( ModalNavigationStore modalNavigationStore, Func<Type, object, MvxViewModel> viewModelFactory ):IModalNavigationControl
    {
        public void PopUp<TViewModel>( int? parameter = null ) where TViewModel : IMvxViewModel
        {
            modalNavigationStore.CurrentModalViewModel = viewModelFactory(typeof(TViewModel), parameter!);
        }

        public void Open<TViewModel>( int? parameter = null ) where TViewModel : IMvxViewModel
        {
            modalNavigationStore.CurrentModalViewModel = ReturnTestViewModel();
        }

        private TestViewModel? ReturnTestViewModel()
        {
            return Mvx.IoCProvider?.Resolve<TestViewModel>();
        }
    }
}
