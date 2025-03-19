using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;
using System.Windows;

namespace PiggsCare.Core.ViewModels.Weaning
{
    public class WeaningDeleteFormViewModel(
        IWeaningStore weaningStore,
        ModalNavigationStore modalNavigationStore,
        IDateConverterService dateConverterService,
        IMessageService messageService )
        :MvxViewModel<int>, IWeaningDeleteFormViewModel
    {
        #region ViewModel Life-Cycle

        public override void Prepare( int parameter )
        {
            _weaningEventId = parameter;
        }

        public override Task Initialize()
        {
            WeaningEvent? record = weaningStore.WeaningEvents.FirstOrDefault(x => x.WeaningEventId == _weaningEventId);
            if (record == null) return base.Initialize();
            PopulateDeleteForm(record);
            _farrowingEventId = record.FarrowingEventId;
            return base.Initialize();
        }

        #endregion

        #region Fields

        private int _farrowingEventId;
        private int _weaningEventId;
        private DateTime _weaningDate;
        private int _numberWeaned;
        private int _malesWeaned;
        private int _femalesWeaned;
        private float _averageWeaningWeight;

        #endregion

        #region Properties

        public DateTime WeaningDate
        {
            get => _weaningDate;
            set
            {
                if (value.Equals(_weaningDate)) return;
                _weaningDate = value;
                RaisePropertyChanged();
            }
        }
        public int NumberWeaned
        {
            get => _numberWeaned;
            set
            {
                if (value == _numberWeaned) return;
                _numberWeaned = value;
                RaisePropertyChanged();
            }
        }
        public int MalesWeaned
        {
            get => _malesWeaned;
            set
            {
                if (value == _malesWeaned) return;
                _malesWeaned = value;
                RaisePropertyChanged();
            }
        }
        public int FemalesWeaned
        {
            get => _femalesWeaned;
            set
            {
                if (value == _femalesWeaned) return;
                _femalesWeaned = value;
                RaisePropertyChanged();
            }
        }
        public float AverageWeaningWeight
        {
            get => _averageWeaningWeight;
            set
            {
                if (value.Equals(_averageWeaningWeight)) return;
                _averageWeaningWeight = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand SubmitRecordCommand => new MvxAsyncCommand(ExecuteSubmitRecord);

        public IMvxCommand CancelRecordCommand => new MvxCommand(ExecuteCancelCommand);

        #endregion

        #region Methods

        private void PopulateDeleteForm( WeaningEvent weaningEvent )
        {
            _weaningDate = dateConverterService.GetDateTime(weaningEvent.WeaningDate);
            _numberWeaned = weaningEvent.NumberWeaned;
            _malesWeaned = weaningEvent.MalesWeaned;
            _femalesWeaned = weaningEvent.FemalesWeaned;
            _averageWeaningWeight = weaningEvent.AverageWeaningWeight;
        }

        private void ExecuteCancelCommand()
        {
            modalNavigationStore.Close();
        }

        private async Task ExecuteSubmitRecord()
        {
            MessageBoxResult confirm = messageService.Show("Are you sure you want to delete the record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.OK)
                await OnDeleteConfirm();
        }

        private async Task OnDeleteConfirm()
        {
            await weaningStore.Remove(_weaningEventId);
            modalNavigationStore.Close();
        }

        #endregion
    }
}
