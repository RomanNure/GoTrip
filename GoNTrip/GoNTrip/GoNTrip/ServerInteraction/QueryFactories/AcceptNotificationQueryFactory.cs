using System.Threading.Tasks;

using GoNTrip.ServerAccess;
using GoNTrip.Model.Notifications;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class AcceptNotificationQueryFactory : QueryFactory
    {
        public async Task<IQuery> AcceptNotification(INotification notification) =>
            new Query(QueryMethod.POST, notification.ServerMethodConfirm, 
                      await ExtractJsonQueryBody<INotification, AcceptNotificationField>(notification));
    }
}
