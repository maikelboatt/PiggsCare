using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MvvmCross.ViewModels;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;
using System.Collections.ObjectModel;

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
        public SeriesCollection SeriesCollection { get; set; }
        public ObservableCollection<string> Labels { get; set; }

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
            SetupPieChart();
            SetupBarChart();
            base.Prepare();
        }

        private void SetupPieChart()
        {

            PointLabel = point => $"{point.Y} ({point.Participation:P})";
        }

        private void SetupBarChart()
        {

            SeriesCollection =
            [
                new ColumnSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { _female, 50, 39, 50 }
                },

                new ColumnSeries
                {
                    Title = "2016",
                    Values = new ChartValues<double> { _male, 56, 42, 48 }
                }
            ];

            Labels = ["Jan", "Feb", "March", "April", "May"];
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
