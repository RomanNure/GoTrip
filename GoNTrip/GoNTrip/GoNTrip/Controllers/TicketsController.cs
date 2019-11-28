using System.Net;
using System.Threading.Tasks;
using System.Security.Cryptography;

using Autofac;

using GoNTrip.Model;
using GoNTrip.Pages;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Controllers
{
    public class TicketsController
    {
        private const string SALT_KEY = "2019GoNTrip2019";

        public async Task<Ticket> GetTicket(Tour tour)
        {
            IQuery getTicketQuery = await App.DI.Resolve<GetTicketQueryFactory>().GetTicket(tour);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(getTicketQuery, App.DI.Resolve<CookieContainer>());
            Participating participating = App.DI.Resolve<IResponseParser>().Parse<Participating, ResponseException>(response);

            return new Ticket(App.DI.Resolve<Session>().CurrentUser, tour, participating, SALT_KEY, MD5.Create());
        }

        public async Task<bool> CheckTicket(TicketChecker ticketChecker)
        {
            IQuery checkTicketQuery = await App.DI.Resolve<CheckTicketQueryFactory>().CheckTicket(ticketChecker);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(checkTicketQuery, App.DI.Resolve<CookieContainer>());
            return App.DI.Resolve<IResponseParser>().Parse<BoolResult, ResponseException>(response).result;
        }
    }
}
