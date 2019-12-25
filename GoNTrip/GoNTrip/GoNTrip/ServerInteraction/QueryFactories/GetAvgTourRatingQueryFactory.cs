using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class GetAvgTourRatingQueryFactory : QueryFactory
    {
        private const string GET_AVG_TOUR_RATING_SERVER_MATHOD_NAME = "tours/get/average";
        public async Task<IQuery> GetAvgTourRating(Tour tour) =>
            new Query(QueryMethod.GET, GET_AVG_TOUR_RATING_SERVER_MATHOD_NAME,
                parameters: await ExtractQueryParameters<Tour, GetTourAvgRating>(tour));
    }
}
