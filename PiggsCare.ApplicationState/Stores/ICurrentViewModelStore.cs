using MvvmCross.ViewModels;
using PiggsCare.Infrastructure.Enums;

namespace PiggsCare.ApplicationState.Stores
{
    public interface ICurrentViewModelStore
    {
        MvxViewModel? CurrentViewModel { get; set; }
        ProcessStage CurrentProcessStage { get; set; }
        event Action? CurrentViewModelChanged;
        event Action? CurrentProcessStageChanged;
    }
}
