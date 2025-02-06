using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;

namespace PiggsCare.Core.Control
{
    public class ModalNavigationControl( ModalNavigationStore modalNavigationStore, Func<Type, object, MvxViewModel> viewModelFactory ):IModalNavigationControl
    {
        public void PopUp<TViewModel>( int? parameter = null ) where TViewModel : IMvxViewModel
        {
            modalNavigationStore.CurrentModalViewModel = viewModelFactory(typeof(TViewModel), parameter!);
        }
    }
}
