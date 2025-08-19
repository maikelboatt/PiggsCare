using MvvmCross.ViewModels;
using PiggsCare.Infrastructure.Enums;

namespace PiggsCare.ApplicationState.Stores
{
    public class CurrentViewModelStore:ICurrentViewModelStore
    {
        #region Properties

        public MvxViewModel? CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                CurrentViewModelChanged?.Invoke();
            }
        }

        public ProcessStage CurrentProcessStage
        {
            get => _currentProcessStage;
            set
            {
                if (_currentProcessStage == value) return;
                _currentProcessStage = value;
                CurrentProcessStageChanged?.Invoke();
            }
        }

        #endregion

        #region Events

        public event Action? CurrentViewModelChanged;
        public event Action? CurrentProcessStageChanged;

        #endregion

        #region Fields

        private MvxViewModel? _currentViewModel;
        private ProcessStage _currentProcessStage = ProcessStage.Health;
        private int _uniqueId;

        #endregion
    }
}
