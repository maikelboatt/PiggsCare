using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Control;
using PiggsCare.Core.Factory;
using PiggsCare.Core.Stores;
using PiggsCare.Core.ViewModels.Weaning;
using PiggsCare.Domain.Models;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels.Farrowing
{
    public class FarrowListingViewModel:MvxViewModel<int>, IFarrowListingViewModel
    {
        #region Constructor

        public FarrowListingViewModel( IFarrowingStore farrowingStore, IModalNavigationControl modalNavigationControl, IViewModelFactory viewModelFactory,
            ICurrentViewModelStore currentViewModelStore )
        {
            _farrowingStore = farrowingStore;
            _modalNavigationControl = modalNavigationControl;
            _viewModelFactory = viewModelFactory;
            _currentViewModelStore = currentViewModelStore;
            _farrowEvents = new MvxObservableCollection<FarrowEvent>(_farrowingStore.FarrowEvents);

            _farrowEvents.CollectionChanged += FarrowEventsOnCollectionChanged;
        }

        #endregion

        #region Event Handler

        private void FarrowEventsOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            RaisePropertyChanged(nameof(FarrowEvents));
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

        #endregion

        #region Fields

        private bool _isLoading;
        private readonly IFarrowingStore _farrowingStore;
        private readonly IModalNavigationControl _modalNavigationControl;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ICurrentViewModelStore _currentViewModelStore;


        private readonly MvxObservableCollection<FarrowEvent> _farrowEvents;

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
                await _farrowingStore.Load(BreedingEventId);
                int loadedCount = _farrowingStore.FarrowEvents.Count();
                Console.WriteLine($"Loaded {loadedCount} Farrowing events");

                foreach (FarrowEvent farrowEvent in _farrowingStore.FarrowEvents)
                {
                    _farrowEvents.Add(farrowEvent);
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
            RaisePropertyChanged(nameof(FarrowEvents));
        }

        private void ExecuteOpenInsertRecordDialog()
        {
            // Open the Farrow Create form dialog
            _modalNavigationControl.PopUp<FarrowingCreateFormViewModel>(BreedingEventId);
        }

        private void ExecuteOpenModifyRecordDialog( int id )
        {
            // Open the Farrow Modify form dialog
            _modalNavigationControl.PopUp<FarrowingModifyFormViewModel>(id);
        }

        private void ExecuteOpenRemoveRecordDialog( int id )
        {
            // Open the Farrow Delete form dialog
            _modalNavigationControl.PopUp<FarrowingDeleteFormViewModel>(id);
        }

        // Opens the WeaningListingViewModel and passes in the breeding event id
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
