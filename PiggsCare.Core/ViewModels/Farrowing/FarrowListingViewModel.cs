using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores;
using PiggsCare.ApplicationState.Stores.Farrowing;
using PiggsCare.Business.Services.Farrowing;
using PiggsCare.Core.Control;
using PiggsCare.Core.Factory;
using PiggsCare.Core.ViewModels.Weaning;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Enums;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels.Farrowing
{
    public class FarrowListingViewModel:MvxViewModel<int>, IFarrowListingViewModel
    {
        private readonly ICurrentViewModelStore _currentViewModelStore;


        private readonly MvxObservableCollection<FarrowEvent> _farrowEvents;
        private readonly IFarrowingService _farrowingService;
        private readonly IFarrowingStore _farrowingStore;
        private readonly ILogger<FarrowListingViewModel> _logger;
        private readonly IModalNavigationControl _modalNavigationControl;
        private readonly IViewModelFactory _viewModelFactory;

        private bool _isLoading;


        #region Constructor

        public FarrowListingViewModel( IFarrowingService farrowingService, IFarrowingStore farrowingStore, IModalNavigationControl modalNavigationControl,
            IViewModelFactory viewModelFactory,
            ICurrentViewModelStore currentViewModelStore, ILogger<FarrowListingViewModel> logger )
        {
            _farrowingService = farrowingService;
            _farrowingStore = farrowingStore;
            _modalNavigationControl = modalNavigationControl;
            _viewModelFactory = viewModelFactory;
            _currentViewModelStore = currentViewModelStore;
            _logger = logger;
            _farrowEvents = new MvxObservableCollection<FarrowEvent>(_farrowingStore.Farrowings);

            _farrowEvents.CollectionChanged += FarrowEventsOnCollectionChanged;

            _farrowingStore.OnFarrowingAdded += FarrowingStoreOnFarrowingAdded;
            _farrowingStore.OnFarrowingUpdated += FarrowingStoreOnOnFarrowingUpdated;
            _farrowingStore.OnFarrowingDeleted += FarrowingStoreOnFarrowingDeleted;
        }

        #endregion

        #region ViewModel Life-Cycle

        public override void Prepare( int parameter )
        {
            BreedingEventId = parameter;
        }

        public override async Task Initialize()
        {
            await LoadFarrowingEventsDetailsAsync();
            await base.Initialize();
        }

        public override void ViewDestroy( bool viewFinishing = true )
        {
            _farrowingStore.OnFarrowingAdded -= FarrowingStoreOnFarrowingAdded;
            _farrowingStore.OnFarrowingUpdated -= FarrowingStoreOnOnFarrowingUpdated;
            _farrowingStore.OnFarrowingDeleted -= FarrowingStoreOnFarrowingDeleted;
            base.ViewDestroy(viewFinishing);
        }

        #endregion

        #region Event Handler

        private void FarrowEventsOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            RaisePropertyChanged(nameof(FarrowEvents));
        }

        private void FarrowingStoreOnFarrowingDeleted( FarrowEvent farrow )
        {
            _farrowEvents.Remove(farrow);
        }

        private void FarrowingStoreOnOnFarrowingUpdated( FarrowEvent farrow )
        {
            // Update the animal in the local collection if it exists
            int index = _farrowEvents.IndexOf(farrow);
            if (index >= 0)
            {
                _farrowEvents.RemoveAt(index);
                _farrowEvents.Insert(index, farrow);
            }
            else
            {
                _logger.LogWarning("Animal with ID {FarrowId} not found in local collection for update.", farrow.FarrowingEventId);
            }
        }

        private void FarrowingStoreOnFarrowingAdded( FarrowEvent farrow )
        {
            _farrowEvents.Add(farrow);
        }

        #endregion

        #region Properties

        public IEnumerable<FarrowEvent> FarrowEvents => _farrowEvents;
        public int BreedingEventId { get; private set; }


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
        public IMvxCommand<int> OpenWeaningEventDialogCommand => new MvxCommand<int>(ExecuteOpenWeaningEventDialog);

        #endregion

        #region Methods

        private async Task LoadFarrowingEventsDetailsAsync()
        {
            IsLoading = true;
            try
            {
                _farrowEvents!.Clear();
                await _farrowingService.GetAllFarrowingsAsync(BreedingEventId);

                foreach (FarrowEvent farrowEvent in _farrowingStore.Farrowings)
                {
                    _farrowEvents.Add(farrowEvent);
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
            RaisePropertyChanged(nameof(FarrowEvents));
        }

        private void ExecuteOpenInsertRecordDialog()
        {
            // Open the Farrow AddAnimal form dialog
            _modalNavigationControl.PopUp<FarrowingCreateFormViewModel>(BreedingEventId);
        }

        private void ExecuteOpenModifyRecordDialog( int id )
        {
            // Open the Farrow UpdateAnimal form dialog
            _modalNavigationControl.PopUp<FarrowingModifyFormViewModel>(id);
        }

        private void ExecuteOpenRemoveRecordDialog( int id )
        {
            // Open the Farrow Delete form dialog
            _modalNavigationControl.PopUp<FarrowingDeleteFormViewModel>(id);
        }

        // Opens the WeaningListingViewModel and passes in the insemination event id
        private void ExecuteOpenWeaningEventDialog( int id )
        {
            WeaningListingViewModel? viewmodel = _viewModelFactory.CreateViewModel<WeaningListingViewModel, int>(id);
            _currentViewModelStore.CurrentViewModel = viewmodel;
            _currentViewModelStore.CurrentProcessStage = ProcessStage.Weaning;
            viewmodel?.Initialize();
        }

        #endregion
    }
}
