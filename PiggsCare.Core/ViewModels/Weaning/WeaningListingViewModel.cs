using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores.Weaning;
using PiggsCare.Business.Services.Weaning;
using PiggsCare.Core.Control;
using PiggsCare.Domain.Models;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels.Weaning
{
    public class WeaningListingViewModel:MvxViewModel<int>, IWeaningListingViewModel
    {
        private readonly ILogger<WeaningListingViewModel> _logger;
        private readonly IModalNavigationControl _modalNavigationControl;

        private readonly IWeaningService _weaningService;
        private readonly IWeaningStore _weaningStore;
        private bool _isLoading;

        #region Constructor

        public WeaningListingViewModel( IWeaningService weaningService, IWeaningStore weaningStore, IModalNavigationControl modalNavigationControl,
            ILogger<WeaningListingViewModel> logger )
        {
            _weaningService = weaningService;
            _weaningStore = weaningStore;
            _modalNavigationControl = modalNavigationControl;
            _logger = logger;
            _weaningEvents.CollectionChanged += WeaningEventsOnCollectionChanged;

            _weaningStore.OnWeaningEventAdded += WeaningStoreOnOnWeaningEventAdded;
            _weaningStore.OnWeaningEventUpdated += WeaningStoreOnOnWeaningEventUpdated;
            _weaningStore.OnWeaningEventDeleted += WeaningStoreOnOnWeaningEventDeleted;
        }

        #endregion

        private MvxObservableCollection<WeaningEvent> _weaningEvents => new(_weaningStore.WeaningEvents);

        #region ViewModel Life-Cycle

        public override void Prepare( int parameter )
        {
            FarrowingEventId = parameter;
        }

        public override async Task Initialize()
        {
            await LoadWeaningEventsDetailsAsync();
            await base.Initialize();
        }

        public override void ViewDestroy( bool viewFinishing = true )
        {
            _weaningStore.OnWeaningEventAdded -= WeaningStoreOnOnWeaningEventAdded;
            _weaningStore.OnWeaningEventUpdated -= WeaningStoreOnOnWeaningEventUpdated;
            _weaningStore.OnWeaningEventDeleted -= WeaningStoreOnOnWeaningEventDeleted;
            base.ViewDestroy(viewFinishing);
        }

        #endregion

        #region Event Handler

        private void WeaningEventsOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            RaisePropertyChanged(nameof(WeaningEvents));
        }

        private void WeaningStoreOnOnWeaningEventDeleted( WeaningEvent weaning )
        {
            _weaningEvents.Remove(weaning);
        }

        private void WeaningStoreOnOnWeaningEventUpdated( WeaningEvent weaning )
        {
            // Update the animal in the local collection if it exists
            int index = _weaningEvents.IndexOf(weaning);
            if (index >= 0)
            {
                _weaningEvents.RemoveAt(index);
                _weaningEvents.Insert(index, weaning);
            }
            else
            {
                _logger.LogWarning("Animal with ID {WeaningId} not found in local collection for update.", weaning.WeaningEventId);
            }
        }

        private void WeaningStoreOnOnWeaningEventAdded( WeaningEvent weaning )
        {
            _weaningEvents.Add(weaning);
        }

        #endregion

        #region Properties

        public int FarrowingEventId { get; private set; }

        public IEnumerable<WeaningEvent> WeaningEvents => _weaningEvents;

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

        private async Task LoadWeaningEventsDetailsAsync()
        {
            IsLoading = true;
            try
            {
                _weaningEvents!.Clear();
                await _weaningService.GetAllWeaningEventAsync(FarrowingEventId);

                foreach (WeaningEvent weaningEvent in _weaningStore.WeaningEvents)
                {
                    _weaningEvents.Add(weaningEvent);
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
            RaisePropertyChanged(nameof(WeaningEvents));
        }

        private void ExecuteOpenInsertRecordDialog()
        {
            // Open the WeaningEvent AddAnimal Form dialog
            _modalNavigationControl.PopUp<WeaningCreateFormViewModel>(FarrowingEventId);
        }

        private void ExecuteOpenModifyRecordDialog( int id )
        {
            // Open the WeaningEvent UpdateAnimal Form dialog
            _modalNavigationControl.PopUp<WeaningModifyFormViewModel>(id);
        }

        private void ExecuteOpenRemoveRecordDialog( int id )
        {
            // Open the WeaningEvent UpdateAnimal Form dialog
            _modalNavigationControl.PopUp<WeaningDeleteFormViewModel>(id);
        }

        #endregion
    }
}
