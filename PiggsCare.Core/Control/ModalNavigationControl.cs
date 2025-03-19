using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;

namespace PiggsCare.Core.Control
{
    public class ModalNavigationControl( ModalNavigationStore modalNavigationStore, Func<Type, object, MvxViewModel> viewModelFactory ):IModalNavigationControl
    {
        public void PopUp<TViewModel>( int? parameter = null ) where TViewModel : IMvxViewModel
        {
            modalNavigationStore.CurrentModalViewModel = viewModelFactory(typeof(TViewModel), parameter!);
        }

        public void PopUp<T>( List<Animal> selectedAnimals ) where T : IMvxViewModel
        {
            modalNavigationStore.CurrentModalViewModel = viewModelFactory(typeof(T), selectedAnimals);
        }
    }
}
