using PiggsCare.Core.Commands;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace PiggsCare.Core.ViewModels
{
    public class NotificationViewModel:ViewModelBase
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
            ClearCommand = new ClearNotificationCommand(this);

            _messages.CollectionChanged += MessagesOnCollectionChanged;
        }

        #region Props

        public ObservableCollection<string> Messages
        {
            get => _messages;
            set => SetField(ref _messages, value);
        }

        #endregion

        #region Commands

        public ICommand ClearCommand { get; private set; }

        #endregion

        private void MessagesOnCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            OnPropertyChanged(nameof(Messages));
        }
    }
}
