using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Control;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels.Synchronization
{
    public class SynchronizationListingViewModel:MvxViewModel, ISynchronizationListingViewModel
    {
        #region Constructor

        public SynchronizationListingViewModel( ISynchronizationEventStore synchronizationEventStore, IModalNavigationControl modalNavigationControl )
        {
            _synchronizationEventStore = synchronizationEventStore;
            _modalNavigationControl = modalNavigationControl;

            _synchronizationEvents.CollectionChanged += SynchronizationEventsOnCollectionChanged;
        }

        #endregion

        #region ViewModel Life-Cycle

        public override async Task Initialize()
        {
            await LoadSynchronizationEventDetailsAsync();
            await base.Initialize();
        }

        #endregion

        #region Event Handlers

        private void SynchronizationEventsOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            RaisePropertyChanged(nameof(SynchronizationEvents));
        }

        #endregion

        #region Fields

        private bool _isLoading;

        private MvxObservableCollection<SynchronizationEvent> _synchronizationEvents => new(_synchronizationEventStore.SynchronizationEvents);
        private readonly ISynchronizationEventStore _synchronizationEventStore;
        private readonly IModalNavigationControl _modalNavigationControl;

        #endregion

        #region Properties

        public int AnimalId { get; private set; } // Property is AnimalId now

        public IEnumerable<SynchronizationEvent> SynchronizationEvents => _synchronizationEvents; // Property is SynchronizationEvents now

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

        private async Task LoadSynchronizationEventDetailsAsync()
        {
            IsLoading = true;
            try
            {
                _synchronizationEvents!.Clear();
                await _synchronizationEventStore.LoadAsync();
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
            RaisePropertyChanged(nameof(SynchronizationEvents)); // Raise property changed for SynchronizationEvents
        }

        private void ExecuteOpenInsertRecordDialog()
        {
            // Open the SynchronizationEventCreateForm dialog
            _modalNavigationControl.PopUp<SynchronizationEventCreateFormViewModel>(1);
        }

        private void ExecuteOpenModifyRecordDialog( int id ) // id is SynchronizationEventId now
        {
            // Open the SynchronizationEventModifyForm dialog
            _modalNavigationControl.PopUp<SynchronizationEventModifyFormViewModel>(id); // Pass SynchronizationEventId to Modify form
        }

        private void ExecuteOpenRemoveRecordDialog( int id ) // id is SynchronizationEventId now
        {
            // Open the SynchronizationEventDeleteForm dialog
            _modalNavigationControl.PopUp<SynchronizationEventDeleteFormViewModel>(id); // Pass SynchronizationEventId to Delete form
        }

        #endregion
    }
}
