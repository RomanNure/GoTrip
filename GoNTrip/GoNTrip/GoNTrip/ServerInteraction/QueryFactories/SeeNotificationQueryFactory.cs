using System.Threading.Tasks;

using GoNTrip.ServerAccess;
using GoNTrip.Model.Notifications;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class SeeNotificationQueryFactory : QueryFactory
    {
        private const string SEE_NOTICATION_SERVER_METHOD_NAME = "notification/check";

        public async Task<IQuery> SeeNotification(INotification notification) =>
            new Query(QueryMethod.POST, SEE_NOTICATION_SERVER_METHOD_NAME, 
                      await ExtractJsonQueryBody<INotification, SeeNotificationField>(notification));
    }
}
