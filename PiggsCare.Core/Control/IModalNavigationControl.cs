using MvvmCross.ViewModels;

namespace PiggsCare.Core.Control
{
    public interface IModalNavigationControl
    {
        void PopUp<TViewModel>( int? parameter = null ) where TViewModel : IMvxViewModel;
    }
}
