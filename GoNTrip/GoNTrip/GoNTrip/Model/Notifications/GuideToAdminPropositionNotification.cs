using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.Model.Notifications
{
    public class GuideToAdminPropositionNotification : NotificationBase
    {
        private const string SERVER_METHOD_CONFIRM = "guide/invitation/accept/admin";
        private const string SERVER_METHOD_REFUSE = "guide/invitation/refuse/admin";

        [RefuseNotificationField]
        [AcceptNotificationField]
        public long tourId => tour.id;

        [RefuseNotificationField]
        [AcceptNotificationField]
        public long guideId => guide.Id;

        [AcceptNotificationField]
        public double sum { get; set; }

        public Guide guide { get; set; }
        public Tour tour { get; set; }

        public GuideToAdminPropositionNotification(string id, bool isChecked, string topic, Tour tour, Guide guide, double sum) : 
            base(id, isChecked, true, topic, SERVER_METHOD_CONFIRM, SERVER_METHOD_REFUSE)
        {
            this.tour = tour;
            this.guide = guide;
            this.sum = sum;
        }

        public override string GetPreviewMessage() => $"{guide.UserProfile.login ?? Constants.UNKNOWN_FILED_VALUE} asks you to approve him to guide tour \"{tour.name}\"";
        public override string GetDetailMessage() => $"{guide.UserProfile.login ?? Constants.UNKNOWN_FILED_VALUE} asks you to approve him to guide tour \"{tour.name}\"." +
                                                     $"He has experiesnce in {guide.Keywords}. He asks for {sum}{Constants.CURRENCY_SYMBOL} salary.";
    }
}
