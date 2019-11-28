using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class NotificationPulseQueryFactory : QueryFactory
    {
        private const string NOTIFICATION_PULSE_SERVER_METHOD_NAME = "notification/new";

        public async Task<IQuery> HasNewNotifications(User user) =>
            new Query(QueryMethod.GET, NOTIFICATION_PULSE_SERVER_METHOD_NAME,
                      parameters: await ExtractQueryParameters<User, NotificationPulseField>(user));
    }
}
