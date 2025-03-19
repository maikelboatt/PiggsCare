using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Control;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels.Weaning
{
    public class WeaningListingViewModel:MvxViewModel<int>, IWeaningListingViewModel
    {
        #region Constructor

        public WeaningListingViewModel( IWeaningStore weaningStore, IModalNavigationControl modalNavigationControl )
        {
            _weaningStore = weaningStore;
            _modalNavigationControl = modalNavigationControl;
            _weaningEvents.CollectionChanged += WeaningEventsOnCollectionChanged;
        }

        #endregion

        #region Event Handler

        private void WeaningEventsOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            RaisePropertyChanged(nameof(WeaningEvents));
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

        #region Fields

        private readonly IWeaningStore _weaningStore;
        private readonly IModalNavigationControl _modalNavigationControl;
        private bool _isLoading;

        private MvxObservableCollection<WeaningEvent> _weaningEvents => new(_weaningStore.WeaningEvents);

        #endregion

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
                await _weaningStore.Load(FarrowingEventId);
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
            RaisePropertyChanged(nameof(WeaningEvents));
        }

        private void ExecuteOpenInsertRecordDialog()
        {
            // Open the WeaningEvent Create Form dialog
            _modalNavigationControl.PopUp<WeaningCreateFormViewModel>(FarrowingEventId);
        }

        private void ExecuteOpenModifyRecordDialog( int id )
        {
            // Open the WeaningEvent Modify Form dialog
            _modalNavigationControl.PopUp<WeaningModifyFormViewModel>(id);
        }

        private void ExecuteOpenRemoveRecordDialog( int id )
        {
            // Open the WeaningEvent Modify Form dialog
            _modalNavigationControl.PopUp<WeaningDeleteFormViewModel>(id);
        }

        #endregion
    }
}
