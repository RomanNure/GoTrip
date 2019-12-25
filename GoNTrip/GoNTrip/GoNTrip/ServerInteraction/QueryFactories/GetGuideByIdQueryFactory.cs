using System.Threading.Tasks;
using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class GetGuideByIdQueryFactory : QueryFactory
    {
        private const string GET_GUIDE_BY_ID_SERVER_METHOD_NAME = "guide/get";
        public async Task<IQuery> GetById(Guide guide) =>
            new Query(QueryMethod.GET, GET_GUIDE_BY_ID_SERVER_METHOD_NAME, parameters: await ExtractQueryParameters<Guide, GetGuideById>(guide));
    }
}
