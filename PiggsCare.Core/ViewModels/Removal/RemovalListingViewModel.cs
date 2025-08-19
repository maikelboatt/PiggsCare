using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores.Removal;
using PiggsCare.Business.Services.Removal;
using PiggsCare.Core.Control;
using PiggsCare.Domain.Models;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels.Removal
{
    public class RemovalListingViewModel:MvxViewModel<int>, IRemovalListingViewModel
    {
        private readonly ILogger<RemovalListingViewModel> _logger;
        private readonly IModalNavigationControl _modalNavigationControl;
        private readonly IRemovalEventStore _removalEventStore;
        private readonly IRemovalService _removalService;

        private bool _isLoading;

        #region Constructor

        public RemovalListingViewModel( IRemovalService removalService, IRemovalEventStore removalEventStore, IModalNavigationControl modalNavigationControl,
            ILogger<RemovalListingViewModel> logger )
        {
            _removalService = removalService;
            _removalEventStore = removalEventStore;
            _modalNavigationControl = modalNavigationControl;
            _logger = logger;

            _removalEvents.CollectionChanged += RemovalEventsOnCollectionChanged;

            _removalEventStore.OnRemovalEventAdded += RemovalEventStoreOnOnRemovalEventAdded;
            _removalEventStore.OnRemovalEventUpdated += RemovalEventStoreOnOnRemovalEventUpdated;
            _removalEventStore.OnRemovalEventDeleted += RemovalEventStoreOnOnRemovalEventDeleted;
        }

        #endregion

        private MvxObservableCollection<RemovalEvent> _removalEvents => new(_removalEventStore.RemovalEvents);

        #region ViewModel Life-Cycle

        public override void Prepare( int parameter )
        {
            AnimalId = parameter;
        }

        public override async Task Initialize()
        {
            await LoadRemovalEventDetailsAsync();
            await base.Initialize();
        }

        public override void ViewDestroy( bool viewFinishing = true )
        {
            _removalEventStore.OnRemovalEventAdded -= RemovalEventStoreOnOnRemovalEventAdded;
            _removalEventStore.OnRemovalEventUpdated -= RemovalEventStoreOnOnRemovalEventUpdated;
            _removalEventStore.OnRemovalEventDeleted -= RemovalEventStoreOnOnRemovalEventDeleted;
            base.ViewDestroy(viewFinishing);
        }

        #endregion

        #region Event Handlers

        private void RemovalEventsOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            RaisePropertyChanged(nameof(RemovalEvents));
        }

        private void RemovalEventStoreOnOnRemovalEventDeleted( RemovalEvent removal )
        {
            _removalEvents.Remove(removal);
        }

        private void RemovalEventStoreOnOnRemovalEventUpdated( RemovalEvent removal )
        {
            // Update the animal in the local collection if it exists
            int index = _removalEvents.IndexOf(removal);
            if (index >= 0)
            {
                _removalEvents.RemoveAt(index);
                _removalEvents.Insert(index, removal);
            }
            else
            {
                _logger.LogWarning("Animal with ID {RemovalEventId} not found in local collection for update.", removal.RemovalEventId);
            }
        }


        private void RemovalEventStoreOnOnRemovalEventAdded( RemovalEvent removal )
        {
            _removalEvents.Add(removal);
        }

        #endregion


        #region Properties

        public int AnimalId { get; private set; }

        public IEnumerable<RemovalEvent> RemovalEvents => _removalEvents;

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

        #endregion

        #region Methods

        private async Task LoadRemovalEventDetailsAsync()
        {
            IsLoading = true;
            try
            {
                _removalEvents!.Clear();
                await _removalService.GetAllRemovalEventsAsync(AnimalId);

                foreach (RemovalEvent removalEvent in _removalEventStore.RemovalEvents)
                {
                    _removalEvents.Add(removalEvent);
                }
                UpdateView();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void UpdateView()
        {
            RaisePropertyChanged(nameof(RemovalEvents)); // Raise property changed for RemovalEvents
        }

        private void ExecuteOpenInsertRecordDialog()
        {
            // Open the RemovalEventCreateForm dialog
            _modalNavigationControl.PopUp<RemovalEventCreateFormViewModel>(AnimalId); // Pass AnimalId to AddAnimal form
        }

        private void ExecuteOpenModifyRecordDialog( int id ) // id is RemovalEventId now
        {
            // Open the RemovalEventModifyForm dialog
            _modalNavigationControl.PopUp<RemovalEventModifyFormViewModel>(id); // Pass RemovalEventId to UpdateAnimal form
        }

        private void ExecuteOpenRemoveRecordDialog( int id ) // id is RemovalEventId now
        {
            // Open the RemovalEventDeleteForm dialog
            _modalNavigationControl.PopUp<RemovalEventDeleteFormViewModel>(id); // Pass RemovalEventId to Delete form
        }

        #endregion
    }
}
