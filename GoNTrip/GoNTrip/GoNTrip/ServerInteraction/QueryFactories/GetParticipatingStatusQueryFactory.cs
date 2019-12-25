using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class GetParticipatingStatusQueryFactory : QueryFactory
    {
        private const string GET_PARTICIPATING_STATUS_SERVER_METHOD_NAME = "participating/status";
        public async Task<IQuery> GetParticipatingStatus(User user, Tour tour) =>
            new Query(QueryMethod.GET, GET_PARTICIPATING_STATUS_SERVER_METHOD_NAME,
                parameters: await ExtractQueryParameters<User, Tour, GetParticipatingStatus>(user, tour));
    }
}
