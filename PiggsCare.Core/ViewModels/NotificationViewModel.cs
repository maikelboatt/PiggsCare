using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace PiggsCare.Core.ViewModels
{
    public class NotificationViewModel:MvxViewModel
    {
        #region Fields

        private readonly INotificationStore _notificationStore;

        #endregion

        private ObservableCollection<string> _messages = [];


        public NotificationViewModel( INotificationStore notificationStore )
        {
            _notificationStore = notificationStore;
            // ClearCommand = new ClearNotificationCommand(this);

            _notificationStore.NotificationsChanged += NotificationStoreOnNotificationsChanged;
            _messages.CollectionChanged += MessagesOnCollectionChanged;
        }

        #region Props

        public ObservableCollection<string> Messages
        {
            get => _messages;
            set => SetProperty(ref _messages, value);
        }

        #endregion

        private void NotificationStoreOnNotificationsChanged()
        {
            RaisePropertyChanged(nameof(Messages));
        }

        public override Task Initialize()
        {
            _messages = new MvxObservableCollection<string>(_notificationStore.Notifications);
            return base.Initialize();
        }

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
