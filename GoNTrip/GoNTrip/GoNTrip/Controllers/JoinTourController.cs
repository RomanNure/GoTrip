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
        public async Task<bool> CheckJoinAbility(User user, Tour tour)
        {
            IQuery checkJoinAbilityQuery = await App.DI.Resolve<CheckTourJoinAbilityQueryFactory>().CheckTourJoinAbility(user, tour);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(checkJoinAbilityQuery);
            return true;
        }

        public async Task<Participating> JoinTour(Tour tour)
        {
            IQuery joinTourQuery = await App.DI.Resolve<JoinTourQueryFactory>().JoinTour(tour);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(joinTourQuery, App.DI.Resolve<CookieContainer>());
            return App.DI.Resolve<IResponseParser>().Parse<Participating>(response);
        }
    }
}
