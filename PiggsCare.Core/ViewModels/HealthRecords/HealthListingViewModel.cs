using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores.Health;
using PiggsCare.Business.Services.Health;
using PiggsCare.Core.Control;
using PiggsCare.Domain.Models;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels.HealthRecords
{
    public class HealthListingViewModel:MvxViewModel<int>, IHealthListingViewModel
    {
        private readonly IHealthRecordStore _healthRecordStore;

        private readonly IHealthService _healthService;
        private readonly ILogger<HealthListingViewModel> _logger;
        private readonly IModalNavigationControl _modalNavigationControl;
        private bool _isLoading;


        public HealthListingViewModel( IHealthService healthService, IHealthRecordStore healthRecordStore, IModalNavigationControl modalNavigationControl,
            ILogger<HealthListingViewModel> logger )
        {
            _healthService = healthService;
            _healthRecordStore = healthRecordStore;
            _modalNavigationControl = modalNavigationControl;
            _logger = logger;
            _healthRecords.CollectionChanged += HealthRecordsOnCollectionChanged;

            _healthRecordStore.OnHealthRecordAdded += HealthRecordStoreOnOnHealthRecordAdded;
            _healthRecordStore.OnHealthRecordUpdated += HealthRecordStoreOnOnHealthRecordUpdated;
            _healthRecordStore.OnHealthRecordDeleted += HealthRecordStoreOnOnHealthRecordDeleted;
        }

        private MvxObservableCollection<HealthRecord> _healthRecords => new(_healthRecordStore.HealthRecords);

        #region ViewModel LifeCycle

        public override void Prepare( int parameter )
        {
            AnimalId = parameter;
        }

        public override async Task Initialize()
        {
            await LoadHealthRecordDetailsAsync();
            await base.Initialize();
        }

        public override void ViewDestroy( bool viewFinishing = true )
        {
            _healthRecordStore.OnHealthRecordAdded -= HealthRecordStoreOnOnHealthRecordAdded;
            _healthRecordStore.OnHealthRecordUpdated -= HealthRecordStoreOnOnHealthRecordUpdated;
            _healthRecordStore.OnHealthRecordDeleted -= HealthRecordStoreOnOnHealthRecordDeleted;
            base.ViewDestroy(viewFinishing);
        }

        #endregion

        #region Event Handlers

        private void HealthRecordsOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            RaisePropertyChanged(nameof(HealthRecords));
        }

        private void HealthRecordStoreOnOnHealthRecordDeleted( HealthRecord health )
        {
            _healthRecords.Remove(health);
        }

        private void HealthRecordStoreOnOnHealthRecordUpdated( HealthRecord health )
        {
            // Update the animal in the local collection if it exists
            int index = _healthRecords.IndexOf(health);
            if (index >= 0)
            {
                _healthRecords.RemoveAt(index);
                _healthRecords.Insert(index, health);
            }
            else
            {
                _logger.LogWarning("Animal with ID {HealthRecordId} not found in local collection for update.", health.HealthRecordId);
            }
        }

        private void HealthRecordStoreOnOnHealthRecordAdded( HealthRecord health )
        {
            _healthRecords.Add(health);
        }

        #endregion

        #region Command

        public IMvxCommand OpenInsertRecordDialogCommand => new MvxCommand(ExecuteOpenInsertRecordDialog);
        public IMvxCommand<int> OpenModifyRecordDialogCommand => new MvxCommand<int>(ExecuteOpenModifyRecordDialog);
        public IMvxCommand<int> OpenRemoveRecordDialogCommand => new MvxCommand<int>(ExecuteOpenRemoveRecordDialog);

        #endregion

        #region Methods

        private async Task LoadHealthRecordDetailsAsync()
        {
            IsLoading = true;
            try
            {
                _healthRecords!.Clear();
                await _healthService.GetAllHealthRecordsAsync(AnimalId);

                foreach (HealthRecord record in _healthRecordStore.HealthRecords)
                {
                    _healthRecords.Add(record);
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
            RaisePropertyChanged(nameof(HealthRecords));
        }

        private void ExecuteOpenInsertRecordDialog()
        {
            _modalNavigationControl.PopUp<HealthRecordCreateFormViewModel>(AnimalId);
        }

        private void ExecuteOpenModifyRecordDialog( int id )
        {
            // Open the AnimalModifyForm dialog
            _modalNavigationControl.PopUp<HealthRecordModifyFormViewModel>(id);
        }

        private void ExecuteOpenRemoveRecordDialog( int id )
        {
            // Open the AnimalDeleteForm dialog
            _modalNavigationControl.PopUp<HealthRecordDeleteFormViewModel>(id);
        }

        #endregion

        #region Properties

        public IEnumerable<HealthRecord> HealthRecords => _healthRecords;
        public int AnimalId { get; private set; }


        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        #endregion
    }
}
