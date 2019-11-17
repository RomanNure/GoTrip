using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class CheckTourJoinAbilityQueryFactory : QueryFactory
    {
        public const string CHECK_TOUR_JOIN_ABILITY_SERVER_METHOD_NAME = "participating/able";

        public async Task<IQuery> CheckTourJoinAbility(User user, Tour tour) =>
            new Query(QueryMethod.GET, CHECK_TOUR_JOIN_ABILITY_SERVER_METHOD_NAME, 
                      parameters: await ExtractQueryParameters<User, Tour, CheckTourJoinAbilityField>(user, tour));
    }
}
