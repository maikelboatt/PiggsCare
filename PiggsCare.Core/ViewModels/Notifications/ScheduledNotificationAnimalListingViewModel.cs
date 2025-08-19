// This is a ViewModel for displaying animals that are included in a scheduleNotification.
// It allows the user to approve insemination events for those animals.

using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores;
using PiggsCare.ApplicationState.Stores.ScheduledNotifications;
using PiggsCare.Business.Services.Insemination;
using PiggsCare.Business.Services.Message;
using PiggsCare.Core.Parameter;
using PiggsCare.Domain.Models;
using System.Windows;

namespace PiggsCare.Core.ViewModels.Notifications
{
    public class ScheduledNotificationAnimalListingViewModel:MvxViewModel<InseminationDetailAnimalList>, IScheduledNotificationAnimalListingViewModel
    {
        private readonly IInseminationService _inseminationService;
        private readonly ILogger<ScheduledNotificationAnimalListingViewModel> _logger;

        /// <summary>
        ///     Service for displaying messages to the user.
        /// </summary>
        private readonly IMessageService _messageService;


        private readonly ModalNavigationStore _modalNavigationStore;

        private readonly IScheduleNotificationAnimalStore _scheduleNotificationAnimalStore;
        private List<int> _animalIds = [];
        private InseminationDetailAnimalList _inseminationDetailAnimalLists;

        private bool _isLoading;

        public ScheduledNotificationAnimalListingViewModel( IScheduleNotificationAnimalStore scheduleNotificationAnimalStore,
            IInseminationService inseminationService, ILogger<ScheduledNotificationAnimalListingViewModel> logger, ModalNavigationStore modalNavigationStore,
            IMessageService messageService )
        {
            _scheduleNotificationAnimalStore = scheduleNotificationAnimalStore;
            _inseminationService = inseminationService;
            _logger = logger;
            _modalNavigationStore = modalNavigationStore;
            _messageService = messageService;

            // Initialize Commands
            ApproveInseminationCommand = new MvxAsyncCommand(ExecuteApproveInsemination);

        }

        private MvxObservableCollection<Animal> _animals => new(_scheduleNotificationAnimalStore.Animals);


        public IEnumerable<Animal> Animals => _animals;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public IMvxAsyncCommand ApproveInseminationCommand { get; }

        public override async Task Initialize()
        {
            LoadScheduledNotificationAnimals();
            await base.Initialize();
        }

        public override void Prepare( InseminationDetailAnimalList parameter )
        {
            _inseminationDetailAnimalLists = parameter;
            _animalIds = parameter.Inseminations.Select(i => i.AnimalId).ToList();
        }

        private async Task ExecuteApproveInsemination( CancellationToken arg )
        {
            foreach (InseminationEvent inseminationEvent in _inseminationDetailAnimalLists.Inseminations)
            {
                await _inseminationService.CreateInseminationEventAsync(inseminationEvent);
            }
            InformUserOfStatus();
            _modalNavigationStore.Close();
        }

        private void InformUserOfStatus()
        {
            _messageService.Show(
                "Insemination has been made for selected animal(s)",
                "Information",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void LoadScheduledNotificationAnimals()
        {
            IsLoading = true;
            try
            {
                _scheduleNotificationAnimalStore.LoadAnimals(_animalIds);

                foreach (Animal animal in _scheduleNotificationAnimalStore.Animals)
                {
                    _animals.Add(animal);
                }

                RaisePropertyChanged(() => Animals);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error loading scheduled notification animals: {ExMessage}", ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
