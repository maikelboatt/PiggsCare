using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores;
using PiggsCare.ApplicationState.Stores.Insemination;
using PiggsCare.Core.Control;
using PiggsCare.Core.Factory;
using PiggsCare.Core.ViewModels.Farrowing;
using PiggsCare.Core.ViewModels.Pregnancy;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Enums;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels.Breeding
{
    public class BreedingBatchListingViewModel:MvxViewModel<int>, IBreedingBatchListingViewModel
    {
        private readonly MvxObservableCollection<InseminationEventWithAnimal> _breedingEvents;
        private readonly ICurrentViewModelStore _currentViewModelStore;
        private readonly IInseminationEventStore _inseminationEventStore;
        private readonly IModalNavigationControl _modalNavigationControl;
        private readonly IViewModelFactory _viewModelFactory;
        private bool _isLoading;

        private int _synchronizationId;


        #region Constructor

        public BreedingBatchListingViewModel( IInseminationEventStore inseminationEventStore, IModalNavigationControl modalNavigationControl, IViewModelFactory viewModelFactory,
            ICurrentViewModelStore currentViewModelStore )
        {
            _inseminationEventStore = inseminationEventStore;
            _modalNavigationControl = modalNavigationControl;
            _viewModelFactory = viewModelFactory;
            _currentViewModelStore = currentViewModelStore;

            _breedingEvents = new MvxObservableCollection<InseminationEventWithAnimal>(_inseminationEventStore.InseminationEventsWithAnimals);

            _breedingEvents.CollectionChanged += BreedingEventsOnCollectionChanged;
        }

        #endregion

        #region Event Handlers

        private void BreedingEventsOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            RaisePropertyChanged(nameof(BreedingEvents));
        }

        #endregion

        #region ViewModel Life Cylce

        public override void Prepare( int parameter )
        {
            _synchronizationId = parameter;
        }

        public override async Task Initialize()
        {
            LoadBreedingEventsDetailsAsync();
            await base.Initialize();
        }

        #endregion

        #region Properties

        public IEnumerable<InseminationEventWithAnimal> BreedingEvents => _breedingEvents;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        #endregion

        #region Command

        public IMvxCommand<int> OpenModifyRecordDialogCommand => new MvxCommand<int>(ExecuteOpenModifyRecordDialog);
        public IMvxCommand<int> OpenRemoveRecordDialogCommand => new MvxCommand<int>(ExecuteOpenRemoveRecordDialog);
        public IMvxCommand<int> OpenPregnancyEventDialogCommand => new MvxCommand<int>(ExecuteOpenPregnancyEventDialog);
        public IMvxCommand<int> OpenFarrowingEventDialogCommand => new MvxCommand<int>(ExecuteOpenFarrowingEventDialog);

        #endregion

        #region Methods

        private void LoadBreedingEventsDetailsAsync()
        {
            IsLoading = true;
            try
            {
                _breedingEvents!.Clear();
                _inseminationEventStore.GetAllInseminationEventsBySynchronizationBatch(_synchronizationId);

                foreach (InseminationEventWithAnimal breedingEvent in _inseminationEventStore.InseminationEventsWithAnimals)
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
