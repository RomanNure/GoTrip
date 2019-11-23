using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class JoinTourQueryFactory : QueryFactory
    {
        private const string JOIN_TOUR_SERVER_METHOD_NAME = "participating/add";

        public async Task<IQuery> JoinTour(Tour tour) =>
            new Query(QueryMethod.POST, JOIN_TOUR_SERVER_METHOD_NAME, await ExtractJsonQueryBody<Tour, JoinTourField>(tour));
    }
}
