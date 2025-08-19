using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores.Pregnancy;
using PiggsCare.Business.Services.Pregnancy;
using PiggsCare.Core.Control;
using PiggsCare.Domain.Models;
using System.Collections.Specialized;
using System.Diagnostics;

namespace PiggsCare.Core.ViewModels.Pregnancy
{
    public class PregnancyListingViewModel:MvxViewModel<int>, IPregnancyListingViewModel
    {
        private readonly ILogger<PregnancyListingViewModel> _logger;
        private readonly IModalNavigationControl _modalNavigationControl;

        private readonly MvxObservableCollection<PregnancyScan> _pregnancies;
        private readonly IPregnancyService _pregnancyService;
        private readonly IPregnancyStore _pregnancyStore;

        private bool _isLoading;


        #region Constructor

        public PregnancyListingViewModel( IPregnancyService pregnancyService, IPregnancyStore pregnancyStore, IModalNavigationControl modalNavigationControl,
            ILogger<PregnancyListingViewModel> logger )
        {
            _pregnancyService = pregnancyService;
            _pregnancyStore = pregnancyStore;
            _modalNavigationControl = modalNavigationControl;
            _logger = logger;

            _pregnancies = new MvxObservableCollection<PregnancyScan>(_pregnancyStore.Pregnancies);
            _pregnancies.CollectionChanged += PregnanciesOnCollectionChanged;

            _pregnancyStore.OnPregnancyAdded += PregnancyStoreOnPregnancyAdded;
            _pregnancyStore.OnPregnancyUpdated += PregnancyStoreOnPregnancyUpdated;
            _pregnancyStore.OnPregnancyDeleted += PregnancyStoreOnPregnancyDeleted;
        }

        #endregion

        #region ViewModel Life-Cycle

        public override void Prepare( int parameter )
        {
            if (parameter <= 0)
            {
                // Log or throw an exception to prevent invalid id propagation
                Debug.WriteLine($"Invalid parameter received: {parameter}");
                throw new ArgumentException("Breeding Event Id must be greater than zero.", nameof(parameter));
            }
            BreedingEventId = parameter;
        }

        public override async Task Initialize()
        {
            await LoadPregnancyScanDetailsAsync();
            await base.Initialize();
        }

        public override void ViewDestroy( bool viewFinishing = true )
        {
            _pregnancyStore.OnPregnancyAdded -= PregnancyStoreOnPregnancyAdded;
            _pregnancyStore.OnPregnancyUpdated -= PregnancyStoreOnPregnancyUpdated;
            _pregnancyStore.OnPregnancyDeleted -= PregnancyStoreOnPregnancyDeleted;
            base.ViewDestroy(viewFinishing);
        }

        #endregion

        #region Event Handlers

        private void PregnanciesOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            RaisePropertyChanged(nameof(Pregnancies));
        }

        private void PregnancyStoreOnPregnancyAdded( PregnancyScan scan )
        {
            _pregnancies.Add(scan);
        }

        private void PregnancyStoreOnPregnancyUpdated( PregnancyScan scan )
        {
            // Update the scan in the local collection if it exists
            int index = _pregnancies.IndexOf(scan);
            if (index >= 0)
            {
                _pregnancies.RemoveAt(index);
                _pregnancies.Insert(index, scan);
            }
            else
            {
                _logger.LogWarning("Animal with ID {ScanId} not found in local collection for update.", scan.ScanId);
            }
        }

        private void PregnancyStoreOnPregnancyDeleted( PregnancyScan scan )
        {
            _pregnancies.Remove(scan);
        }

        #endregion

        #region Properties

        public int BreedingEventId { get; private set; }

        public IEnumerable<PregnancyScan> Pregnancies => _pregnancies;

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

        private async Task LoadPregnancyScanDetailsAsync()
        {
            IsLoading = true;
            try
            {
                _pregnancies!.Clear();
                await _pregnancyService.GetAllPregnancyScansAsync(BreedingEventId);

                foreach (PregnancyScan pregnancyScan in _pregnancyStore.Pregnancies)
                {
                    _pregnancies.Add(pregnancyScan);
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
            RaisePropertyChanged(nameof(Pregnancies));
        }

        private void ExecuteOpenInsertRecordDialog()
        {
            // Open the PregnancyCreateForm dialog
            _modalNavigationControl.PopUp<PregnancyCreateFormViewModel>(BreedingEventId);
        }

        private void ExecuteOpenModifyRecordDialog( int id )
        {
            // Open the PregnancyModifyForm dialog
            _modalNavigationControl.PopUp<PregnancyModifyFormViewModel>(id);
        }

        private void ExecuteOpenRemoveRecordDialog( int id )
        {
            // Open the PregnancyDeleteForm dialog
            _modalNavigationControl.PopUp<PregnancyDeleteFormViewModel>(id);
        }

        #endregion
    }
}
