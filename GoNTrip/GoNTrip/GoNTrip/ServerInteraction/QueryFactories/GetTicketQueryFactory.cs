using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;
using System.Threading.Tasks;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class GetTicketQueryFactory : QueryFactory
    {
        private const string GET_TICKET_SERVER_METHOD_NAME = "participating/get";

        public async Task<IQuery> GetTicket(Tour tour) =>
            new Query(QueryMethod.GET, GET_TICKET_SERVER_METHOD_NAME, 
                      parameters: await ExtractQueryParameters<Tour, GetTicketField>(tour));
    }
}
