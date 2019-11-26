using System.Threading.Tasks;

using GoNTrip.ServerAccess;
using GoNTrip.Model.Notifications;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class DeleteNotificationQuryFactory : QueryFactory
    {
        private const string DELETE_NOTIFICATION_SERVER_METHOD_NAME = "notification/delete";

        public async Task<IQuery> DeleteNotification(INotification notification) =>
            new Query(QueryMethod.POST, DELETE_NOTIFICATION_SERVER_METHOD_NAME,
                      await ExtractJsonQueryBody<INotification, DeleteNotificationField>(notification));
    }
}
