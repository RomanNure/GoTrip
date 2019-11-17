using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class GetTourMembersQueryFactory : QueryFactory
    {
        public const string GET_TOUR_MEMBERS_SERVER_METHOD_NAME = "tours/get/users";

        public async Task<IQuery> GetTourMembers(Tour tour) => 
            new Query(QueryMethod.GET, GET_TOUR_MEMBERS_SERVER_METHOD_NAME, parameters: await ExtractQueryParameters<Tour, GetTourMembersField>(tour));
    }
}
