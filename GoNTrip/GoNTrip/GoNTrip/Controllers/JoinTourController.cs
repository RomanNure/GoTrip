using System.Net;
using System.Threading.Tasks;

using Autofac;

using GoNTrip.Model;
using GoNTrip.Pages;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Controllers
{
    public class JoinTourController
    {
        public async Task JoinTour(Tour tour)
        {
            //string sessionId = App.DI.Resolve<CookieContainer>().GetCookies(Se)
            IQuery joinTourQuery = await App.DI.Resolve<JoinTourQueryFactory>().JoinTour(tour);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(joinTourQuery, App.DI.Resolve<CookieContainer>());
            //App.DI.Resolve<IResponseParser>().Parse<>
        }
    }
}
