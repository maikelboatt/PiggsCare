using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Control;
using PiggsCare.Core.Stores;
using PiggsCare.Core.ViewModels.Synchronization;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace PiggsCare.Core.ViewModels.Animals
{
    public class AnimalListingViewModel:MvxViewModel
    {
        #region ViewModelLifeCycle

        public override async Task Initialize()
        {
            await LoadAnimalsAsync();
            SetSelectedAnimalToFalse();
            await base.Initialize();
        }

        #endregion

        #region Event Handlers

        private void AnimalsOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            RaisePropertyChanged(nameof(Animals));
        }

        #endregion

        #region Constructor

        public AnimalListingViewModel( IModalNavigationControl modalNavigationControl, IAnimalStore animalStore, IMessageService messageService )
        {
            _modalNavigationControl = modalNavigationControl;
            _animalStore = animalStore;
            _messageService = messageService;
            _animals = new MvxObservableCollection<Animal>(_animalStore.Animals);
            _animals.CollectionChanged += AnimalsOnCollectionChanged;

            _animalStore.OnSave += AnimalStoreOnOnSave;
            _animalStore.OnUpdate += AnimalStoreOnOnUpdate;
            _animalStore.OnDelete += AnimalStoreOnOnDelete;


            AnimalCollectionView = CollectionViewSource.GetDefaultView(_animals);
            AnimalCollectionView.Filter = FilterAnimals;
        }

        private void AnimalStoreOnOnUpdate( Animal obj )
        {
            RaisePropertyChanged(nameof(Animals));
        }

        private void AnimalStoreOnOnDelete( int id )
        {
            Animal? animal = _animals.FirstOrDefault(a => a.AnimalId == id);
            if (animal == null) return;
            _animals.Remove(animal);
            RaisePropertyChanged(nameof(Animals));
        }

        private void AnimalStoreOnOnSave( Animal animal )
        {
            _animals.Add(animal);
        }

        private bool FilterAnimals( object obj )
        {
            if (obj is Animal animal)
            {
                // return animal.Breed.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase);
                return animal.Name.ToString().Contains(SearchText, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        #endregion

        #region Commands

        public IMvxCommand OpenInsertRecordDialogCommand => new MvxCommand(ExecuteOpenInsertRecordDialog);
        public IMvxCommand<int> OpenModifyRecordDialogCommand => new MvxCommand<int>(ExecuteOpenModifyRecordDialog);
        public IMvxCommand<int> OpenRemoveRecordDialogCommand => new MvxCommand<int>(ExecuteOpenRemoveRecordDialog);

        public IMvxCommand<int> OpenAnimalDetailsDialogCommand => new MvxCommand<int>(ExecuteOpenAnimalDetailsDialog);

        // Command for Creating Synchronization Event
        public IMvxCommand OpenSynchronizationEventDialogCommand => new MvxCommand(ExecuteOpenSynchronizationEventDialog);

        #endregion

        #region Fields

        private readonly MvxObservableCollection<Animal> _animals;

        private readonly IModalNavigationControl _modalNavigationControl;
        private readonly IAnimalStore _animalStore;
        private readonly IMessageService _messageService;
        private bool _isLoading;
        private string _searchText = string.Empty;

        #endregion

        #region Properties

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public List<Animal> SelectedAnimals => [.. Animals.Where(animal => animal.IsSelected)];

        public ICollectionView AnimalCollectionView { get; }

        public IEnumerable<Animal> Animals => _animals;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                AnimalCollectionView.Refresh();
            }
        }

        #endregion

        #region Methods

        private async Task LoadAnimalsAsync()
        {
            IsLoading = true;
            try
            {
                await _animalStore.Load();
                _animals.Clear();
                // Add each animal loaded from the store
                foreach (Animal animal in _animalStore.Animals)
                {
                    _animals.Add(animal);
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
            RaisePropertyChanged(nameof(Animals));
            RaisePropertyChanged(nameof(SelectedAnimals));
        }

        private void SetSelectedAnimalToFalse()
        {
            foreach (Animal animal in SelectedAnimals)
            {
                animal.IsSelected = false;
            }
        }

        private void ExecuteOpenInsertRecordDialog()
        {
            // Open the AnimalCreateForm dialog
            _modalNavigationControl.PopUp<AnimalCreateFormViewModel>(6);
        }

        private void ExecuteOpenSynchronizationEventDialog()
        {
            // Ensure there is at least one selected animal.
            List<Animal> selected = SelectedAnimals;
            if (selected.Count == 0)
            {
                _messageService.Show("Please select one or more animals for synchronization", "Notification", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            // Open the synchronization event creation form, passing the list of selected animals.
            _modalNavigationControl.PopUp<SynchronizationEventCreateFormViewModel>(selected.ToList());
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
