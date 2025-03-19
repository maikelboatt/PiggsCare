using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Control;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using System.Collections.Specialized;
using System.Diagnostics;

namespace PiggsCare.Core.ViewModels.Pregnancy
{
    public class PregnancyListingViewModel:MvxViewModel<int>, IPregnancyListingViewModel
    {
        #region Constructor

        public PregnancyListingViewModel( IPregnancyStore pregnancyStore, IModalNavigationControl modalNavigationControl )
        {
            _pregnancyStore = pregnancyStore;
            _modalNavigationControl = modalNavigationControl;

            _pregnancies = new MvxObservableCollection<PregnancyScan>(_pregnancyStore.PregnancyScans);

            _pregnancies.CollectionChanged += PregnanciesOnCollectionChanged;
        }

        #endregion

        #region Event Handlers

        private void PregnanciesOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            RaisePropertyChanged(nameof(Pregnancies));
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

        #endregion

        #region Fields

        private bool _isLoading;

        private readonly MvxObservableCollection<PregnancyScan> _pregnancies;
        private readonly IPregnancyStore _pregnancyStore;
        private readonly IModalNavigationControl _modalNavigationControl;

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
                await _pregnancyStore.Load(BreedingEventId);
                int loadedCount = _pregnancyStore.PregnancyScans.Count();
                Console.WriteLine($"Loaded {loadedCount} records");

                foreach (PregnancyScan pregnancyScan in _pregnancyStore.PregnancyScans)
                {
                    _pregnancies.Add(pregnancyScan);
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
