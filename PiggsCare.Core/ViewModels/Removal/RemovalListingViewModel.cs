using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Control;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels.Removal
{
    public class RemovalListingViewModel:MvxViewModel<int>, IRemovalListingViewModel
    {
        #region Constructor

        public RemovalListingViewModel( IRemovalEventStore removalEventStore, IModalNavigationControl modalNavigationControl )
        {
            _removalEventStore = removalEventStore;
            _modalNavigationControl = modalNavigationControl;

            _removalEvents.CollectionChanged += RemovalEventsOnCollectionChanged;
        }

        #endregion

        #region Event Handlers

        private void RemovalEventsOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            RaisePropertyChanged(nameof(RemovalEvents));
        }

        #endregion

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

        #endregion

        #region Fields

        private bool _isLoading;

        private MvxObservableCollection<RemovalEvent> _removalEvents => new(_removalEventStore.RemovalEvents);
        private readonly IRemovalEventStore _removalEventStore;
        private readonly IModalNavigationControl _modalNavigationControl;

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
                await _removalEventStore.LoadAsync(AnimalId); // Load with AnimalId now
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
            RaisePropertyChanged(nameof(RemovalEvents)); // Raise property changed for RemovalEvents
        }

        private void ExecuteOpenInsertRecordDialog()
        {
            // Open the RemovalEventCreateForm dialog
            _modalNavigationControl.PopUp<RemovalEventCreateFormViewModel>(AnimalId); // Pass AnimalId to Create form
        }

        private void ExecuteOpenModifyRecordDialog( int id ) // id is RemovalEventId now
        {
            // Open the RemovalEventModifyForm dialog
            _modalNavigationControl.PopUp<RemovalEventModifyFormViewModel>(id); // Pass RemovalEventId to Modify form
        }

        private void ExecuteOpenRemoveRecordDialog( int id ) // id is RemovalEventId now
        {
            // Open the RemovalEventDeleteForm dialog
            _modalNavigationControl.PopUp<RemovalEventDeleteFormViewModel>(id); // Pass RemovalEventId to Delete form
        }

        #endregion
    }
}
