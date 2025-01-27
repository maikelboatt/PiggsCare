using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Control;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels
{
    public class AnimalListingViewModel:MvxViewModel
    {
        #region Constructor

        public AnimalListingViewModel( IAnimalService animalService, IModalNavigationControl modalNavigationControl )
        {
            _animalService = animalService;
            _modalNavigationControl = modalNavigationControl;
            _animals.CollectionChanged += AnimalsOnCollectionChanged;
        }

        #endregion

        #region Fields

        private readonly MvxObservableCollection<Animal> _animals = [];
        private readonly IAnimalService _animalService;

        // private static readonly IEnumerable<Animal> _animalsEnumerable = [];
        private readonly IModalNavigationControl _modalNavigationControl;
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

        public IMvxAsyncCommand LoadAnimalsCommand => new MvxAsyncCommand(TestCrudOperations);

        #endregion

        #region Methods

        private void AnimalsOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            RaisePropertyChanged(nameof(Animals));
        }


        private async Task LoadAnimals()
        {
            _animals.Clear();
            IEnumerable<Animal> animals = await _animalService.GetAllAnimalsAsync();
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
                await LoadAnimals();
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
            // _modalNavigationControl.PopUp<TestViewModel>(1);
            // _modalNavigationControl.Open<TestViewModel>();
        }

        #endregion
    }
}
