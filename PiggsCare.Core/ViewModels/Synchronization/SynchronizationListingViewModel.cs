using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores.Synchronization;
using PiggsCare.Business.Services.Synchronization;
using PiggsCare.Core.Control;
using PiggsCare.Core.Factory;
using PiggsCare.Domain.Models;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels.Synchronization
{
    /// <summary>
    ///     ViewModel for handling synchronization listing operations.
    /// </summary>
    public class SynchronizationListingViewModel:MvxViewModel, ISynchronizationListingViewModel
    {
        private readonly ILogger<SynchronizationListingViewModel> _logger;
        private readonly IModalNavigationControl _modalNavigationControl;
        private readonly ISynchronizationEventStore _synchronizationEventStore;
        private readonly ISynchronizationService _synchronizationService;
        private readonly IViewModelFactory _viewModelFactory;

        private bool _isLoading;


        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="SynchronizationListingViewModel" /> class.
        /// </summary>
        /// <param name="synchronizationEventStore" >The synchronization event store.</param>
        /// <param name="modalNavigationControl" >The modal navigation control.</param>
        /// <param name="viewModelFactory" >The view model factory.</param>
        public SynchronizationListingViewModel( ISynchronizationService synchronizationService, ISynchronizationEventStore synchronizationEventStore,
            IModalNavigationControl modalNavigationControl, IViewModelFactory viewModelFactory, ILogger<SynchronizationListingViewModel> logger )
        {
            _synchronizationService = synchronizationService;
            _synchronizationEventStore = synchronizationEventStore;
            _modalNavigationControl = modalNavigationControl;
            _viewModelFactory = viewModelFactory;
            _logger = logger;

            _synchronizationEvents.CollectionChanged += SynchronizationEventsOnCollectionChanged;
            _synchronizationEventStore.OnSynchronizationEventAdded += SynchronizationEventsOnSynchronizationEventAdded;
            _synchronizationEventStore.OnSynchronizationEventUpdated += SynchronizationEventStoreOnOnSynchronizationEventUpdated;
            _synchronizationEventStore.OnSynchronizationEventDeleted += SynchronizationEventStoreOnOnSynchronizationEventDeleted;
        }

        #endregion

        private MvxObservableCollection<SynchronizationEvent> _synchronizationEvents => new(_synchronizationEventStore.SynchronizationEvents);

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
            _synchronizationEvents.CollectionChanged -= SynchronizationEventsOnCollectionChanged;
            _synchronizationEvents.CollectionChanged -= SynchronizationEventsOnCollectionChanged;
            _synchronizationEventStore.OnSynchronizationEventAdded -= SynchronizationEventsOnSynchronizationEventAdded;
            _synchronizationEventStore.OnSynchronizationEventUpdated -= SynchronizationEventStoreOnOnSynchronizationEventUpdated;
            _synchronizationEventStore.OnSynchronizationEventDeleted -= SynchronizationEventStoreOnOnSynchronizationEventDeleted;
            base.ViewDestroy(viewFinishing);
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

        private void SynchronizationEventStoreOnOnSynchronizationEventDeleted( SynchronizationEvent synchronization )
        {
            _synchronizationEvents.Remove(synchronization);
        }

        private void SynchronizationEventStoreOnOnSynchronizationEventUpdated( SynchronizationEvent synchronization )
        {
            // Update the synchronization in the local collection if it exists
            int index = _synchronizationEvents.IndexOf(synchronization);
            if (index >= 0)
            {
                _synchronizationEvents.RemoveAt(index);
                _synchronizationEvents.Insert(index, synchronization);
            }
            else
            {
                _logger.LogWarning("Animal with ID {Synchronization} not found in local collection for update.", synchronization.SynchronizationEventId);
            }
        }

        private void SynchronizationEventsOnSynchronizationEventAdded( SynchronizationEvent synchronization )
        {
            _synchronizationEvents.Add(synchronization);
        }

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
                await _synchronizationService.GetAllSynchronizationEventsAsync();
                foreach (SynchronizationEvent synchronizationEvent in _synchronizationEventStore.SynchronizationEvents)
                {
                    _synchronizationEvents.Add(synchronizationEvent);
                }
                {

                }
                UpdateView();
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
            _modalNavigationControl.PopUp<SynchronizationEventModifyFormViewModel>(id); // Pass SynchronizationEventId to UpdateAnimal form
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
