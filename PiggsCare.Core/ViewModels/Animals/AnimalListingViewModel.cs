using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Control;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels.Animals
{
    public class AnimalListingViewModel:MvxViewModel
    {
        #region Constructor

        public AnimalListingViewModel( IModalNavigationControl modalNavigationControl, IAnimalStore animalStore, IHealthRecordService healthRecordService )
        {
            _modalNavigationControl = modalNavigationControl;
            _animalStore = animalStore;
            _healthRecordService = healthRecordService;
            _animals.CollectionChanged += AnimalsOnCollectionChanged;

            animalStore.OnLoad += AnimalStoreOnOnLoad;
            animalStore.OnSave += AnimalStoreOnOnSave;
            animalStore.OnUpdate += AnimalStoreOnOnUpdate;
            animalStore.OnDelete += AnimalStoreOnOnDelete;
        }

        #endregion

        #region ViewModelLifeCycle

        public override async Task Initialize()
        {
            await LoadAnimalsAsync();
            await base.Initialize();
        }

        #endregion

        #region Commands

        public IMvxCommand OpenInsertRecordDialogCommand => new MvxCommand(ExecuteOpenInsertRecordDialog);
        public IMvxCommand<int> OpenModifyRecordDialogCommand => new MvxCommand<int>(ExecuteOpenModifyRecordDialog);
        public IMvxCommand<int> OpenRemoveRecordDialogCommand => new MvxCommand<int>(ExecuteOpenRemoveRecordDialog);

        public IMvxCommand<int> OpenAnimalDetailsDialogCommand => new MvxCommand<int>(ExecuteOpenAnimalDetailsDialog);

        #endregion

        #region Event Handlers

        private void AnimalStoreOnOnSave( Animal obj )
        {
            RaisePropertyChanged(nameof(Animals));
        }

        private void AnimalStoreOnOnUpdate( Animal obj )
        {
            RaisePropertyChanged(nameof(Animals));
        }

        private void AnimalStoreOnOnDelete( int obj )
        {
            RaisePropertyChanged(nameof(Animals));
        }

        private void AnimalStoreOnOnLoad()
        {
            RaisePropertyChanged(nameof(Animals));
        }

        private void AnimalsOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            RaisePropertyChanged(nameof(Animals));
        }

        #endregion

        #region Fields

        private MvxObservableCollection<Animal> _animals => new(_animalStore.Animals);

        private readonly IModalNavigationControl _modalNavigationControl;
        private readonly IAnimalStore _animalStore;
        private readonly IHealthRecordService _healthRecordService;
        private bool _isLoading;

        #endregion

        #region Properties

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public IEnumerable<Animal> Animals => _animals;
        // public ICollectionView Animals { get; private set; }

        #endregion

        #region Methods

        private async Task LoadAnimalsAsync()
        {
            IsLoading = true;
            try
            {
                await _animalStore.Load();
                _animals.Clear();
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
            RaisePropertyChanged(nameof(Animals));
        }

        private void ExecuteOpenInsertRecordDialog()
        {
            // Open the AnimalCreateForm dialog
            _modalNavigationControl.PopUp<AnimalCreateFormViewModel>(6);
        }

        private void ExecuteOpenModifyRecordDialog( int id )
        {
            // Open the AnimalModifyForm dialog
            _modalNavigationControl.PopUp<AnimalModifyFormViewModel>(id);
        }

        private void ExecuteOpenRemoveRecordDialog( int id )
        {
            // Open the AnimalDeleteForm dialog
            _modalNavigationControl.PopUp<AnimalDeleteFormViewModel>(id);
        }

        private void ExecuteOpenAnimalDetailsDialog( int id )
        {
            _modalNavigationControl.PopUp<SelectedAnimalDetailsViewModel>(id);
        }

        #endregion
    }
}
