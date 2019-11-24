using GoNTrip.Model;
using System.Threading.Tasks;

using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class GetUserNotificationsQueryFactory : QueryFactory
    {
        private const string GET_USER_NOTIFICATIONS_SERVER_METHOD_NAME = "notification/get/user";

        public async Task<IQuery> GetUserNotifications(User user) =>
            new Query(QueryMethod.GET, GET_USER_NOTIFICATIONS_SERVER_METHOD_NAME,
                      parameters: await ExtractQueryParameters<User, GetNotificationsField>(user));
    }
}
