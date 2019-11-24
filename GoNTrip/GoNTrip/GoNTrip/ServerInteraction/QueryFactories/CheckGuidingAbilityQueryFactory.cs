using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class CheckGuidingAbilityQueryFactory : QueryFactory
    {
        private const string CHECK_GUIDING_ABILITY_SERVER_METHOD_NAME = "guide/invitation/able";

        public async Task<IQuery> CheckGuidingAbility(Tour tour, Guide guide) =>
            new Query(QueryMethod.GET, CHECK_GUIDING_ABILITY_SERVER_METHOD_NAME,
                      parameters: await ExtractQueryParameters<Tour, Guide, CheckTourGuidingAbilityField>(tour, guide));
    }
}
