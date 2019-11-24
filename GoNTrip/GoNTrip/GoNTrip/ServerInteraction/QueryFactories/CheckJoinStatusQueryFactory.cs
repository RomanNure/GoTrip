using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class CheckJoinStatusQueryFactory : QueryFactory
    {
        private const string CHECK_JOIN_STATUS_SERVER_METHOD_NAME = "participating/check";

        public async Task<IQuery> CheckJoinStatus(LiqpayPayment payment) =>
            new Query(QueryMethod.GET, CHECK_JOIN_STATUS_SERVER_METHOD_NAME,
                      parameters: await ExtractQueryParameters<LiqpayPayment, CheckJoinStatusField>(payment));
    }
}
