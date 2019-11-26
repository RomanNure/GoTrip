using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.Model.Notifications
{
    public abstract class NotificationBase : INotification
    {
        public NotificationBase(string id, bool isChecked, bool confirmable, string topic, string serverMethodConfirm, string serverMethodRefuse)
        {
            Id = id;
            IsChecked = isChecked;
            IsConfirmable = confirmable;
            Topic = topic;

            ServerMethodConfirm = serverMethodConfirm;
            ServerMethodRefuse = serverMethodRefuse;
        }

        [RefuseNotificationField("notificationId")]
        [DeleteNotificationField("notificationId")]
        [SeeNotificationField("notificationId")]
        public string Id { get; protected set; }

        public bool IsChecked { get; protected set; }
        public bool IsConfirmable { get; protected set; }
        public string Topic { get; protected set; }

        public string ServerMethodConfirm { get; protected set; }
        public string ServerMethodRefuse { get; protected set; }

        public abstract string GetPreviewMessage();
        public abstract string GetDetailMessage();
    }
}
