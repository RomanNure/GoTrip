using System.Threading.Tasks;

using Autofac;

using GoNTrip.Model;
using GoNTrip.Pages;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.QueryFactories;

namespace GoNTrip.Controllers
{
    public class PayController
    {
        private const string LIQPAY_SERVER = "https://www.liqpay.ua/";

        public async Task<object> PayForTour(Tour tour, Card card)
        {   
            LiqpayPayment payment = new LiqpayPayment(App.DI.Resolve<Session>().CurrentUser, tour, card);
            IQuery payQuery = await App.DI.Resolve<PayQueryFactory>().Pay(payment);

            IServerCommunicator server = App.DI.Resolve<IServerCommunicator>();

            string temp = server.ServerURL;
            server.ServerURL = LIQPAY_SERVER;

            IServerResponse response = await server.SendQuery(payQuery);
            server.ServerURL = temp;

            return null;
        }
    }
}
