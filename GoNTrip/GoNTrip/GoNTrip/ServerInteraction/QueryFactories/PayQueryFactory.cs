using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class PayQueryFactory : QueryFactory
    {
        private const string PAY_SER_VER_METHOD_NAME = "api/request";

        public async Task<IQuery> Pay(LiqpayPayment payment) =>
            new Query(QueryMethod.POST_URLENCODED, PAY_SER_VER_METHOD_NAME,
                      parameters: await ExtractQueryParameters<LiqpayPayment, PayField>(payment));
    }
}
