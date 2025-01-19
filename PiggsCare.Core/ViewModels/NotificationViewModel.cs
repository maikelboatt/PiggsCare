using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels
{
    public class NotificationViewModel:MvxViewModel
    {
        private ObservableCollection<string> _messages =
        [
            "A new record has been successfully created!",
            "Record already exists!",
            "Record has been successfully updated!",
            "Record has been successfully deleted!",
            "404: The record was not found!",
            "500: Internal Server Error!"
        ];

        public NotificationViewModel()
        {
            // ClearCommand = new ClearNotificationCommand(this);

            _messages.CollectionChanged += MessagesOnCollectionChanged;
        }

        #region Props

        public ObservableCollection<string> Messages
        {
            get => _messages;
            set => SetProperty(ref _messages, value);
        }

        #endregion

        private void MessagesOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            RaisePropertyChanged(nameof(Messages));
        }

        #region Commands

        public IMvxCommand ClearCommand => new MvxCommand(ClearCommandExecute);

        private void ClearCommandExecute()
        {
            _messages.Clear();
        }

        #endregion
    }
}
