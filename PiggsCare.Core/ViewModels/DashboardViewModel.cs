using LiveCharts;
using LiveCharts.Defaults;
using MvvmCross.ViewModels;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;

namespace PiggsCare.Core.ViewModels
{
    public class DashboardViewModel( IAnimalService animalService ):MvxViewModel
    {
        private double _female;
        private IChartValues _femaleValues;

        private bool _isLoading;
        private double _male;
        private IChartValues _maleValues;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public Func<ChartPoint, string> PointLabel { get; set; }

        public IChartValues FemaleValues
        {
            get
            {
                ObservableValue count = new(_female);
                return new ChartValues<ObservableValue>([count]);
            }
            set => SetProperty(ref _femaleValues, value);
        }
        public IChartValues MaleValues
        {
            get
            {
                ObservableValue count = new(_male);
                return new ChartValues<ObservableValue>([count]);
            }
            set => SetProperty(ref _maleValues, value);
        }

        public override void Prepare()
        {
            PointLabel = point => $"{point.Y} ({point.Participation:P})";
            base.Prepare();
        }

        public override async Task Initialize()
        {
            await LoadAnimalDetails();
            await base.Initialize();
        }

        private async Task LoadAnimalDetails()
        {
            IsLoading = true;
            try
            {
                IEnumerable<Animal> animal = await animalService.GetAllAnimalsAsync();
                Animal[] enumerable = animal as Animal[] ?? animal.ToArray();
                _male = enumerable.Count(pig => pig.Gender == Gender.Male);
                _female = enumerable.Count(pig => pig.Gender == Gender.Female);
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
    }
}
