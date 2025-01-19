using MvvmCross.ViewModels;

namespace PiggsCare.Core.ViewModels
{
    public interface IMainViewModel
    {
        MvxViewModel CurrentViewModel { get; set; }
    }
}
