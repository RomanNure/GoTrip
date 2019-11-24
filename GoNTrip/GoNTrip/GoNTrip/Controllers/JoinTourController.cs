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
            return App.DI.Resolve<IResponseParser>().Parse<ParticipateAbility, ResponseException>(response).able;
        }

        public async Task<bool> JoinPrepare(Tour tour, LiqpayPayment payment)
        {
            IQuery prepareJoinQuery = await App.DI.Resolve<JoinPrepareQueryFactory>().PrepareJoin(App.DI.Resolve<Session>().CurrentUser, tour, payment);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(prepareJoinQuery, App.DI.Resolve<CookieContainer>());
            return App.DI.Resolve<IResponseParser>().Parse<ParticipateAbility, ResponseException>(response).able;
        }

        public async Task<JoinTourStatus> JoinCheckStatus(LiqpayPayment payment)
        {
            IQuery joinCheckStatusQuery = await App.DI.Resolve<CheckJoinStatusQueryFactory>().CheckJoinStatus(payment);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(joinCheckStatusQuery);
            return App.DI.Resolve<IResponseParser>().Parse<JoinTourStatus, ResponseException>(response);
        }

        public async Task<Participating> JoinTour(Tour tour)
        {
            IQuery joinTourQuery = await App.DI.Resolve<JoinTourQueryFactory>().JoinTour(tour);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(joinTourQuery, App.DI.Resolve<CookieContainer>());
            return App.DI.Resolve<IResponseParser>().Parse<Participating, ResponseException>(response);
        }
    }
}
