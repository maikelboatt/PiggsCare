using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Control;
using PiggsCare.Core.Factory;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels.Synchronization
{
    /// <summary>
    ///     ViewModel for handling synchronization listing operations.
    /// </summary>
    public class SynchronizationListingViewModel:MvxViewModel, ISynchronizationListingViewModel
    {
        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="SynchronizationListingViewModel" /> class.
        /// </summary>
        /// <param name="synchronizationEventStore" >The synchronization event store.</param>
        /// <param name="modalNavigationControl" >The modal navigation control.</param>
        /// <param name="viewModelFactory" >The view model factory.</param>
        /// <param name="currentViewModelStore" >The current view model store.</param>
        public SynchronizationListingViewModel( ISynchronizationEventStore synchronizationEventStore, IModalNavigationControl modalNavigationControl, IViewModelFactory viewModelFactory,
            ICurrentViewModelStore currentViewModelStore )
        {
            _synchronizationEventStore = synchronizationEventStore;
            _modalNavigationControl = modalNavigationControl;
            _viewModelFactory = viewModelFactory;
            _currentViewModelStore = currentViewModelStore;

            _synchronizationEvents.CollectionChanged += SynchronizationEventsOnCollectionChanged;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        ///     Handles the collection changed event for synchronization events.
        /// </summary>
        /// <param name="sender" >The event sender.</param>
        /// <param name="e" >The event arguments.</param>
        private void SynchronizationEventsOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            RaisePropertyChanged(nameof(SynchronizationEvents));
        }

        #endregion

        #region ViewModel Life-Cycle

        /// <summary>
        ///     Initializes the ViewModel asynchronously.
        /// </summary>
        public override async Task Initialize()
        {
            await LoadSynchronizationEventDetailsAsync();
            await base.Initialize();
        }

        public override void ViewDestroy( bool viewFinishing = true )
        {
            base.ViewDestroy(viewFinishing);
            _synchronizationEvents.CollectionChanged -= SynchronizationEventsOnCollectionChanged;
        }

        #endregion

        #region Fields

        private bool _isLoading;

        private MvxObservableCollection<SynchronizationEvent> _synchronizationEvents => new(_synchronizationEventStore.SynchronizationEvents);
        private readonly ISynchronizationEventStore _synchronizationEventStore;
        private readonly IModalNavigationControl _modalNavigationControl;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ICurrentViewModelStore _currentViewModelStore;

        #endregion

        #region Properties

        public int AnimalId { get; private set; } // Property is AnimalId now

        public IEnumerable<SynchronizationEvent> SynchronizationEvents => _synchronizationEvents; // Property is SynchronizationEvents now

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        #endregion

        #region Command

        public IMvxCommand OpenInsertRecordDialogCommand => new MvxCommand(ExecuteOpenInsertRecordDialog);
        public IMvxCommand<int> OpenModifyRecordDialogCommand => new MvxCommand<int>(ExecuteOpenModifyRecordDialog);
        public IMvxCommand<int> OpenRemoveRecordDialogCommand => new MvxCommand<int>(ExecuteOpenRemoveRecordDialog);
        public IMvxCommand<int> OpenBatchDetailsDialogCommand => new MvxCommand<int>(ExecuteOpenBatchDetailsDialog);

        #endregion

        #region Methods

        /// <summary>
        ///     Loads the synchronization event details asynchronously.
        /// </summary>
        private async Task LoadSynchronizationEventDetailsAsync()
        {
            IsLoading = true;
            try
            {
                _synchronizationEvents!.Clear();
                await _synchronizationEventStore.LoadAsync();
                UpdateView();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        ///     Updates the view by raising property changed for synchronization events.
        /// </summary>
        private void UpdateView()
        {
            RaisePropertyChanged(nameof(SynchronizationEvents));
        }

        private void ExecuteOpenInsertRecordDialog()
        {
            // Open the SynchronizationEventCreateForm dialog
            _modalNavigationControl.PopUp<SynchronizationEventCreateFormViewModel>(1);
        }

        private void ExecuteOpenModifyRecordDialog( int id ) // id is SynchronizationEventId now
        {
            // Open the SynchronizationEventModifyForm dialog
            _modalNavigationControl.PopUp<SynchronizationEventModifyFormViewModel>(id); // Pass SynchronizationEventId to Modify form
        }

        private void ExecuteOpenRemoveRecordDialog( int id ) // id is SynchronizationEventId now
        {
            // Open the SynchronizationEventDeleteForm dialog
            _modalNavigationControl.PopUp<SynchronizationEventDeleteFormViewModel>(id); // Pass SynchronizationEventId to Delete form
        }

        /// <summary>
        ///     Executes the command to open the batch details dialog.
        /// </summary>
        /// <param name="synchronizationId" >The synchronization event ID.</param>
        private void ExecuteOpenBatchDetailsDialog( int synchronizationId )
        {
            _modalNavigationControl.PopUp<SelectedBatchDetailsViewModel>(synchronizationId);
        }

        #endregion
    }
}
