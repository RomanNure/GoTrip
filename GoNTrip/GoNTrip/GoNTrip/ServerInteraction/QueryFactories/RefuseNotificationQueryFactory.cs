using System.Threading.Tasks;

using GoNTrip.ServerAccess;
using GoNTrip.Model.Notifications;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class RefuseNotificationQueryFactory : QueryFactory
    {
        private const string REFUSE_NOTIFICATION_SERVER_METHOD_NAME = "guide/invitation/refuse";

        public async Task<IQuery> RefuseNotification(INotification notification) =>
            new Query(QueryMethod.POST, REFUSE_NOTIFICATION_SERVER_METHOD_NAME,
                      await ExtractJsonQueryBody<INotification, RefuseNotificationField>(notification));
    }
}
