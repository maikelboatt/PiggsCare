using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Control;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels.HealthRecords
{
    public class HealthListingViewModel:MvxViewModel<int>, IHealthListingViewModel
    {
        public HealthListingViewModel( IHealthRecordStore healthRecordStore, IModalNavigationControl modalNavigationControl )
        {
            _healthRecordStore = healthRecordStore;
            _modalNavigationControl = modalNavigationControl;
            _healthRecords.CollectionChanged += HealthRecordsOnCollectionChanged;
        }

        #region Event Handlers

        private void HealthRecordsOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            RaisePropertyChanged(nameof(HealthRecords));
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
                await _healthRecordStore.Load(AnimalId);
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

        #region ViewModel LifeCycle

        public override void Prepare( int parameter )
        {
            Console.WriteLine(parameter);
            AnimalId = parameter;
        }

        public override async Task Initialize()
        {
            await LoadHealthRecordDetailsAsync();
            await base.Initialize();
        }

        #endregion

        #region Properties

        public IEnumerable<HealthRecord> HealthRecords => _healthRecords;
        public int AnimalId { get; private set; }

        public DateTime RecordDate
        {
            get => _recordDate;
            set => SetProperty(ref _recordDate, value);
        }
        public string Diagnosis
        {
            get => _diagnosis;
            set => SetProperty(ref _diagnosis, value);
        }
        public string Treatment
        {
            get => _treatment;
            set => SetProperty(ref _treatment, value);
        }
        public string Outcome
        {
            get => _outcome;
            set => SetProperty(ref _outcome, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        #endregion


        #region Fields

        private DateTime _recordDate;
        private string _diagnosis;
        private string _treatment;
        private string _outcome;
        private readonly IHealthRecordStore _healthRecordStore;
        private readonly IModalNavigationControl _modalNavigationControl;
        private bool _isLoading;

        private MvxObservableCollection<HealthRecord> _healthRecords => new(_healthRecordStore.HealthRecords);

        #endregion
    }
}
