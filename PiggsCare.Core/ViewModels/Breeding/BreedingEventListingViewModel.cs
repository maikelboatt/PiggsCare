using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Control;
using PiggsCare.Core.Factory;
using PiggsCare.Core.Stores;
using PiggsCare.Core.ViewModels.Farrowing;
using PiggsCare.Core.ViewModels.Pregnancy;
using PiggsCare.Domain.Models;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels.Breeding
{
    public class BreedingEventListingViewModel:MvxViewModel<int>, IBreedingEventListingViewModel
    {
        #region Constructor

        public BreedingEventListingViewModel( IBreedingEventStore breedingEventStore, IModalNavigationControl modalNavigationControl, IViewModelFactory viewModelFactory,
            ICurrentViewModelStore currentViewModelStore )
        {
            _breedingEventStore = breedingEventStore;
            _modalNavigationControl = modalNavigationControl;
            _viewModelFactory = viewModelFactory;
            _currentViewModelStore = currentViewModelStore;

            _breedingEvents = new MvxObservableCollection<BreedingEvent>(_breedingEventStore.BreedingEvents);

            _breedingEvents.CollectionChanged += BreedingEventsOnCollectionChanged;
            _breedingEventStore.OnUpdate += BreedingEventStoreOnOnUpdate;
        }

        #endregion

        #region Event Handlers

        private void BreedingEventsOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            RaisePropertyChanged(nameof(BreedingEvents));
        }

        private void BreedingEventStoreOnOnUpdate( BreedingEvent obj )
        {
            RaisePropertyChanged(nameof(BreedingEvent));
        }

        #endregion

        #region Properties

        public IEnumerable<BreedingEvent> BreedingEvents => _breedingEvents;

        public int AnimalId { get; private set; }

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
                _breedingEvents!.Clear();
                await _breedingEventStore.Load(AnimalId);

                foreach (BreedingEvent breedingEvent in _breedingEventStore.BreedingEvents)
                {
                    _breedingEvents.Add(breedingEvent);
                }

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

        private void UpdateView()
        {
            RaisePropertyChanged(nameof(BreedingEvents));
        }

        private void ExecuteOpenInsertRecordDialog()
        {
            _modalNavigationControl.PopUp<BreedingEventCreateFormViewModel>(AnimalId);
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

        // Opens the PregnancyListingViewModel and passes in the breeding event id
        private void ExecuteOpenPregnancyEventDialog( int id )
        {
            // if (id <= 0)
            // {
            //     Console.WriteLine("Attempted to navigate with an invalid breeding event id: " + id);
            //     return; // or handle the error appropriately
            // }
            PregnancyListingViewModel? viewmodel = _viewModelFactory.CreateViewModel<PregnancyListingViewModel, int>(id);
            _currentViewModelStore.CurrentViewModel = viewmodel;
            _currentViewModelStore.CurrentProcessStage = ProcessStage.Pregnancy;
            viewmodel?.Initialize();
        }

        // Opens the FarrowingListingViewModel and passes in the breeding event id
        private void ExecuteOpenFarrowingEventDialog( int id )
        {
            FarrowListingViewModel? viewmodel = _viewModelFactory.CreateViewModel<FarrowListingViewModel, int>(id);
            _currentViewModelStore.CurrentViewModel = viewmodel;
            _currentViewModelStore.CurrentProcessStage = ProcessStage.Farrowing;
            viewmodel?.Initialize();
        }

        #endregion

        #region Fields

        private readonly MvxObservableCollection<BreedingEvent> _breedingEvents;
        private bool _isLoading;
        private readonly IBreedingEventStore _breedingEventStore;
        private readonly IModalNavigationControl _modalNavigationControl;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ICurrentViewModelStore _currentViewModelStore;

        #endregion

        #region ViewModel LifeCycle

        public override void Prepare( int parameter )
        {
            AnimalId = parameter;
        }

        public override async Task Initialize()
        {
            await LoadBreedingEventsDetailsAsync();
            await base.Initialize();
        }

        #endregion
    }
}
