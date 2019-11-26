namespace GoNTrip.Model.Notifications
{
    public class NewsNotification : NotificationBase
    {
        public NewsNotification(string id, bool isChecked, string topic) : base(id, isChecked, false, topic, "", "") { }

        public override string GetDetailMessage() => $"{Topic}";
        public override string GetPreviewMessage() => $"{Topic}";
    }
}
