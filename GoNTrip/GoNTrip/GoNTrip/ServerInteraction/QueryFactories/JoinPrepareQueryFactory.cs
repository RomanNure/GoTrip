using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class JoinPrepareQueryFactory : QueryFactory
    {
        private const string JOIN_PREPARE_SERVER_METHOD_NAME = "participating/prepare";

        public async Task<IQuery> PrepareJoin(User user, Tour tour, LiqpayPayment payment) =>
            new Query(QueryMethod.POST, JOIN_PREPARE_SERVER_METHOD_NAME,
                      await ExtractJsonQueryBody<User, Tour, LiqpayPayment, JoinPrepareField>(user, tour, payment));
    }
}
