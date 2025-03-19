using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;
using System.Windows;

namespace PiggsCare.Core.ViewModels.Farrowing
{
    public class FarrowingDeleteFormViewModel(
        IFarrowingStore farrowingStore,
        ModalNavigationStore modalNavigationStore,
        IMessageService messageService,
        IDateConverterService dateConverterService )
        :MvxViewModel<int>, IFarrowingDeleteFormViewModel
    {
        #region ViewModel Life-Cycle

        public override void Prepare( int parameter )
        {
            _farrowingEventId = parameter;
        }

        public override Task Initialize()
        {
            FarrowEvent? record = farrowingStore.FarrowEvents.FirstOrDefault(x => x.FarrowingEventId == _farrowingEventId);
            if (record == null) return base.Initialize();
            PopulateDeleteForm(record);
            _breedingEventId = record.BreedingEventId;
            return base.Initialize();
        }

        #endregion

        #region Fields

        private int _farrowingEventId;
        private int _breedingEventId;
        private string _birthStatus = string.Empty;
        private DateTime _farrowDate;
        private int _litterSize;
        private int _bornAlive;
        private int _bornDead;
        private int _mummified;

        #endregion

        #region Methods

        private void PopulateDeleteForm( FarrowEvent record )
        {
            _farrowDate = dateConverterService.GetDateTime(record.FarrowDate);
            _litterSize = record.LitterSize;
            _bornAlive = record.BornAlive;
            _bornDead = record.BordDead;
            _mummified = record.Mummified;
        }

        private async Task ExecuteSubmitRecord()
        {
            MessageBoxResult confirm = messageService.Show("Are you sure you want to delete the record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.OK)
                await OnDeleteConfirm();
        }

        private async Task OnDeleteConfirm()
        {
            await farrowingStore.Remove(_farrowingEventId);
            modalNavigationStore.Close();
        }

        private void ExecuteCancelCommand()
        {
            modalNavigationStore.Close();
        }

        #endregion

        #region Properties

        public string BirthStatus
        {
            get => _birthStatus;
            set => SetProperty(ref _birthStatus, value);
        }

        public DateTime FarrowDate
        {
            get => _farrowDate;
            set => SetProperty(ref _farrowDate, value);
        }
        public int LitterSize
        {
            get => _litterSize;
            set => SetProperty(ref _litterSize, value);
        }

        public int BornAlive
        {
            get => _bornAlive;
            set => SetProperty(ref _bornAlive, value);
        }
        public int BornDead
        {
            get => _bornDead;
            set => SetProperty(ref _bornDead, value);
        }
        public int Mummified
        {
            get => _mummified;
            set => SetProperty(ref _mummified, value);
        }

        public BirthStatus BirthStatusCollection { get; set; }

        #endregion

        #region Commands

        public IMvxAsyncCommand SubmitRecordCommand => new MvxAsyncCommand(ExecuteSubmitRecord);


        public IMvxCommand CancelRecordCommand => new MvxCommand(ExecuteCancelCommand);

        #endregion
    }
}
