using System.Threading.Tasks;

using GoNTrip.ServerAccess;
using GoNTrip.Model.Notifications;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class RefuseNotificationQueryFactory : QueryFactory
    {
        public async Task<IQuery> RefuseNotification(INotification notification) =>
            new Query(QueryMethod.POST, notification.ServerMethodRefuse, 
                await ExtractJsonQueryBody<INotification, RefuseNotificationField>(notification));
    }
}
