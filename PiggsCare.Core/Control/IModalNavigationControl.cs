using MvvmCross.ViewModels;
using PiggsCare.Core.Parameter;
using PiggsCare.Domain.Models;

namespace PiggsCare.Core.Control
{
    public interface IModalNavigationControl
    {
        void PopUp<TViewModel>( int? parameter = null ) where TViewModel : IMvxViewModel;

        void PopUp<T>( List<Animal> selectedAnimals ) where T : IMvxViewModel;

        void PopUp<T>( InseminationDetailAnimalList inseminationDetailAnimalList ) where T : IMvxViewModel;

        void PopUp<T>( ScheduledNotification scheduledNotifications ) where T : IMvxViewModel;
    }
}
