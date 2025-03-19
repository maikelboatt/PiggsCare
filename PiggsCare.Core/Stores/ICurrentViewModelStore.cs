using MvvmCross.ViewModels;
using PiggsCare.Core.ViewModels;

namespace PiggsCare.Core.Stores
{
    public interface ICurrentViewModelStore
    {
        MvxViewModel? CurrentViewModel { get; set; }
        ProcessStage CurrentProcessStage { get; set; }
        event Action? CurrentViewModelChanged;
        event Action? CurrentProcessStageChanged;
    }
}
