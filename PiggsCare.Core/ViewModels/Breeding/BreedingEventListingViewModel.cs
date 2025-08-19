using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores;
using PiggsCare.ApplicationState.Stores.Insemination;
using PiggsCare.Business.Services.Insemination;
using PiggsCare.Core.Control;
using PiggsCare.Core.Factory;
using PiggsCare.Core.ViewModels.Farrowing;
using PiggsCare.Core.ViewModels.Pregnancy;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Enums;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels.Breeding
{
    public class BreedingEventListingViewModel:MvxViewModel<int>, IBreedingEventListingViewModel
    {
        private readonly MvxObservableCollection<InseminationEvent> _breedingEvents;
        private readonly ICurrentViewModelStore _currentViewModelStore;
        private readonly IInseminationEventStore _inseminationEventStore;
        private readonly IInseminationService _inseminationService;
        private readonly ILogger<BreedingEventListingViewModel> _logger;
        private readonly IModalNavigationControl _modalNavigationControl;
        private readonly IViewModelFactory _viewModelFactory;
        private int _animalId;

        private bool _isLoading;


        #region Constructor

        public BreedingEventListingViewModel( IInseminationService inseminationService, IInseminationEventStore inseminationEventStore, IModalNavigationControl modalNavigationControl,
            IViewModelFactory viewModelFactory,
            ICurrentViewModelStore currentViewModelStore, ILogger<BreedingEventListingViewModel> logger )
        {
            _inseminationService = inseminationService;
            _inseminationEventStore = inseminationEventStore;
            _modalNavigationControl = modalNavigationControl;
            _viewModelFactory = viewModelFactory;
            _currentViewModelStore = currentViewModelStore;
            _logger = logger;

            _breedingEvents = new MvxObservableCollection<InseminationEvent>(_inseminationEventStore.InseminationEvents);
            _breedingEvents.CollectionChanged += BreedingEventsOnCollectionChanged;


            _inseminationEventStore.OnInseminationEventUpdated += InseminationEventStoreOnOnUpdate;
            _inseminationEventStore.OnInseminationEventAdded += InseminationEventStoreOnOnInseminationEventAdded;
            _inseminationEventStore.OnInseminationEventDeleted += InseminationEventStoreOnOnInseminationEventDeleted;
        }

        #endregion

        #region ViewModel LifeCycle

        public override void Prepare( int parameter )
        {
            _animalId = parameter;
        }

        public override async Task Initialize()
        {
            await LoadBreedingEventsDetailsAsync();
            await base.Initialize();
        }

        public override void ViewDestroy( bool viewFinishing = true )
        {
            _inseminationEventStore.OnInseminationEventUpdated -= InseminationEventStoreOnOnUpdate;
            _inseminationEventStore.OnInseminationEventAdded -= InseminationEventStoreOnOnInseminationEventAdded;
            _inseminationEventStore.OnInseminationEventDeleted -= InseminationEventStoreOnOnInseminationEventDeleted;
            base.ViewDestroy(viewFinishing);
        }

        #endregion

        #region Event Handlers

        private void BreedingEventsOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            RaisePropertyChanged(nameof(BreedingEvents));
        }

        private void InseminationEventStoreOnOnUpdate( InseminationEvent insemination )
        {
            // Update the animal in the local collection if it exists
            int index = _breedingEvents.IndexOf(insemination);
            if (index >= 0)
            {
                _breedingEvents.RemoveAt(index);
                _breedingEvents.Insert(index, insemination);
            }
            else
            {
                _logger.LogWarning("Animal with ID {InseminationId} not found in local collection for update.", insemination.BreedingEventId);
            }
        }

        private void InseminationEventStoreOnOnInseminationEventDeleted( InseminationEvent insemination )
        {
            _breedingEvents.Remove(insemination);
        }

        private void InseminationEventStoreOnOnInseminationEventAdded( InseminationEvent insemination )
        {
            _breedingEvents.Add(insemination);
        }

        #endregion

        #region Properties

        public IEnumerable<InseminationEvent> BreedingEvents => _breedingEvents;

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
        public IMvxCommand<int> OpenPregnancyEventDialogCommand => new MvxCommand<int>(ExecuteOpenPregnancyEventDialog);
        public IMvxCommand<int> OpenFarrowingEventDialogCommand => new MvxCommand<int>(ExecuteOpenFarrowingEventDialog);

        #endregion

        #region Methods

        private async Task LoadBreedingEventsDetailsAsync()
        {
            IsLoading = true;
            try
            {
                _breedingEvents.Clear();
                await _inseminationService.GetAllInseminationEventsAsync(_animalId);

                foreach (InseminationEvent breedingEvent in _inseminationEventStore.InseminationEvents)
                {
                    _breedingEvents.Add(breedingEvent);
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
            RaisePropertyChanged(nameof(BreedingEvents));
        }

        private void ExecuteOpenInsertRecordDialog()
        {
            _modalNavigationControl.PopUp<BreedingEventCreateFormViewModel>(_animalId);
        }

        private void ExecuteOpenModifyRecordDialog( int id )
        {
            // Open the AnimalModifyForm dialog
            _modalNavigationControl.PopUp<BreedingEventModifyFormViewModel>(id);

        }

        private void ExecuteOpenRemoveRecordDialog( int id )
        {
            // Open the AnimalDeleteForm dialog
            _modalNavigationControl.PopUp<BreedingEventDeleteFormViewModel>(id);
        }

        // Opens the PregnancyListingViewModel and passes in the insemination event id
        private void ExecuteOpenPregnancyEventDialog( int id )
        {
            // if (id <= 0)
            // {
            //     Console.WriteLine("Attempted to navigate with an invalid insemination event id: " + id);
            //     return; // or handle the error appropriately
            // }
            PregnancyListingViewModel? viewmodel = _viewModelFactory.CreateViewModel<PregnancyListingViewModel, int>(id);
            _currentViewModelStore.CurrentViewModel = viewmodel;
            _currentViewModelStore.CurrentProcessStage = ProcessStage.Pregnancy;
            viewmodel?.Initialize();
        }

        // Opens the FarrowingListingViewModel and passes in the insemination event id
        private void ExecuteOpenFarrowingEventDialog( int id )
        {
            FarrowListingViewModel? viewmodel = _viewModelFactory.CreateViewModel<FarrowListingViewModel, int>(id);
            _currentViewModelStore.CurrentViewModel = viewmodel;
            _currentViewModelStore.CurrentProcessStage = ProcessStage.Farrowing;
            viewmodel?.Initialize();
        }

        #endregion
    }
}
