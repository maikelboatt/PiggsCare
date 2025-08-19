using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores;
using PiggsCare.Core.Parameter;
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

        public void PopUp<T>( InseminationDetailAnimalList inseminationDetailAnimalList ) where T : IMvxViewModel
        {
            modalNavigationStore.CurrentModalViewModel = viewModelFactory(typeof(T), inseminationDetailAnimalList);
        }

        public void PopUp<T>( IEnumerable<ScheduledNotification> scheduledNotifications ) where T : IMvxViewModel
        {
            modalNavigationStore.CurrentModalViewModel = viewModelFactory(typeof(T), scheduledNotifications);
        }

        public void PopUp<T>( ScheduledNotification scheduledNotifications ) where T : IMvxViewModel
        {
            modalNavigationStore.CurrentModalViewModel = viewModelFactory(typeof(T), scheduledNotifications);
        }
    }
}
