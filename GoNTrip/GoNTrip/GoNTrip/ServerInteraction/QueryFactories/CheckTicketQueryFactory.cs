using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class CheckTicketQueryFactory : QueryFactory
    {
        private const string CHECK_TICKET_SERVER_METHOD_NAME = "participating/hash";

        public async Task<IQuery> CheckTicket(TicketChecker ticketChecker) =>
            new Query(QueryMethod.POST, CHECK_TICKET_SERVER_METHOD_NAME,
                      await ExtractJsonQueryBody<TicketChecker, CheckTicketField>(ticketChecker));
    }
}
