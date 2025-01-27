using MvvmCross.ViewModels;

namespace PiggsCare.Core.Factory
{
    public interface IViewModelFactory
    {
        TViewModel? CreateViewModel<TViewModel>() where TViewModel : MvxViewModel;

        TViewModel? CreateViewModel<TViewModel, TParameter>( TParameter parameter ) where TViewModel : MvxViewModel;
    }
}
