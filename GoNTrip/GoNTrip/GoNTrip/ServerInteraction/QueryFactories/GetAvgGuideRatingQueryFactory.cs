using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class GetAvgGuideRatingQueryFactory : QueryFactory
    {
        private const string GET_AVG_GUIDE_RATING_SERVER_MATHOD_NAME = "guide/get/average";
        public async Task<IQuery> GetAvgGuideRating(Guide guide) =>
            new Query(QueryMethod.GET, GET_AVG_GUIDE_RATING_SERVER_MATHOD_NAME,
                parameters: await ExtractQueryParameters<Guide, GetGuideAvgRating>(guide));
    }
}
