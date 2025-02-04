using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Control;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels
{
    public class AnimalListingViewModel:MvxViewModel
    {
        #region Constructor

        public AnimalListingViewModel( IModalNavigationControl modalNavigationControl, IAnimalStore animalStore )
        {
            _modalNavigationControl = modalNavigationControl;
            _animalStore = animalStore;
            _animals.CollectionChanged += AnimalsOnCollectionChanged;

            animalStore.OnLoad += AnimalStoreOnOnLoad;
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand LoadAnimalsCommand => new MvxAsyncCommand(TestCrudOperations);
        public IMvxAsyncCommand InsertRecordCommand => new MvxAsyncCommand(ExecuteInsertRecord);

        #endregion

        #region Event Handlers

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

        private readonly MvxObservableCollection<Animal> _animals = [];

        private readonly IModalNavigationControl _modalNavigationControl;
        private readonly IAnimalStore _animalStore;
        private bool _isLoading;

        #endregion

        #region ViewModelLifeCycle

        public override void Prepare()
        {
            Console.WriteLine("Prepare animals listing");
            base.Prepare();
        }

        public override async Task Initialize()
        {
            await LoadAnimalsAsync();
            await base.Initialize();
        }

        #endregion

        #region Properties

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public IEnumerable<Animal> Animals => _animals;

        #endregion

        #region Methods

        private void LoadAnimals()
        {
            _animals.Clear();
            IEnumerable<Animal> animals = _animalStore.Animals;
            foreach (Animal animal in animals)
            {
                _animals.Add(animal);
            }
        }

        private async Task LoadAnimalsAsync()
        {
            IsLoading = true;
            try
            {
                await _animalStore.Load();
                LoadAnimals();
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

        private async Task ExecuteInsertRecord()
        {
            _modalNavigationControl.PopUp<AnimalCreateFormViewModel>(7);
            // TODO Add functionality for inserting record
        }

        private async Task TestCrudOperations()
        {
            // const Gender male = Gender.Male;
            // string? maleString = Enum.GetName(male);
            // Animal record = new(102, "Oestrus", DateTime.Now, 33, maleString, 34.16F);
            // await _animalService.CreateAnimalAsync(record);

            // Animal? animal = await _animalService.GetAnimalByNameAsync(102);
            //
            // // foreach (Animal animal in animals)
            // Console.WriteLine(animal);
            _modalNavigationControl.PopUp<AnimalCreateFormViewModel>(7);
            // _modalNavigationControl.Open<TestViewModel>();
        }

        #endregion
    }
}
