using MvvmCross.ViewModels;

namespace PiggsCare.Core.Stores
{
    public class ModalNavigationStore
    {
        // Field that contains the current modal
        private MvxViewModel? _currentModalViewModel;

        /// <summary>
        ///     Property that accesses and retrieves the current modal viewmodel
        ///     It raises the CurrentModalViewModelChanged event
        /// </summary>
        public MvxViewModel? CurrentModalViewModel
        {
            get => _currentModalViewModel;
            set
            {
                _currentModalViewModel = value;
                CurrentModalViewModelChanged?.Invoke();
            }
        }

        /// <summary>
        ///     Flag that specifies whether the modal should be open or closed
        /// </summary>
        public bool IsOpen => CurrentModalViewModel != null;

        // Event that fires when the current modal changes
        public event Action? CurrentModalViewModelChanged;

        /// <summary>
        ///     Closes the open modal
        /// </summary>
        public void Close()
        {
            CurrentModalViewModel = null;
        }
    }
}
