using MvvmCross.ViewModels;
using PiggsCare.Domain.Models;

namespace PiggsCare.Core.Control
{
    public interface IModalNavigationControl
    {
        void PopUp<TViewModel>( int? parameter = null ) where TViewModel : IMvxViewModel;

        void PopUp<T>( List<Animal> selectedAnimals ) where T : IMvxViewModel;
    }
}
