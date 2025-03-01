using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Control;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels.Breeding
{
    public class BreedingEventListingViewModel:MvxViewModel<int>, IBreedingEventListingViewModel
    {
        #region Constructor

        public BreedingEventListingViewModel( IBreedingEventStore breedingEventStore, IModalNavigationControl modalNavigationControl, IBreedingEventService breedingEventService )
        {
            _breedingEventStore = breedingEventStore;
            _modalNavigationControl = modalNavigationControl;
            _breedingEventService = breedingEventService;

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

        #endregion

        #region Methods

        private async Task LoadBreedingEventsDetailsAsync()
        {
            IsLoading = true;
            try
            {
                _breedingEvents!.Clear();
                await _breedingEventStore.Load(AnimalId);
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

        #endregion

        #region Fields

        private MvxObservableCollection<BreedingEvent> _breedingEvents => new(_breedingEventStore.BreedingEvents);
        private bool _isLoading;
        private readonly IBreedingEventStore _breedingEventStore;
        private readonly IModalNavigationControl _modalNavigationControl;
        private readonly IBreedingEventService _breedingEventService;

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
