using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.Model.Notifications
{
    public class GuidingOfferNotification : NotificationBase
    {
        private const string SERVER_METHOD_CONFIRM = "guide/invitation/accept";
        private const string SERVER_METHOD_REFUSE = "guide/invitation/refuse";

        [RefuseNotificationField]
        [AcceptNotificationField]
        public long tourId { get => tour.id; }

        [RefuseNotificationField]
        [AcceptNotificationField]
        public long guideId { get; private set; }

        public Company company { get; set; }
        public Tour tour { get; set; }

        [AcceptNotificationField]
        public double sum { get; set; }

        public GuidingOfferNotification(string id, bool isChecked, string topic, Company company, Tour tour, long guideId, double sum) : 
            base(id, isChecked, true, topic, SERVER_METHOD_CONFIRM, SERVER_METHOD_REFUSE)
        {
            this.company = company;
            this.guideId = guideId;
            this.tour = tour;
            this.sum = sum;
        }

        public override string GetPreviewMessage() => $"{company.name ?? Constants.UNKNOWN_FILED_VALUE} invites you to guide tour";
        public override string GetDetailMessage() => $"{company.name ?? Constants.UNKNOWN_FILED_VALUE} invites you to guide tour " +
                                                     $"\"{tour.name ?? Constants.UNKNOWN_FILED_VALUE}\" for {sum}{Constants.CURRENCY_SYMBOL}";
    }
}
