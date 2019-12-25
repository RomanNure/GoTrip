using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using GoNTrip.Pages;
using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.Model.FilterSortSearch.Tour;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Controllers
{
    public class TourController
    {
        public async Task<List<Tour>> GetTours(TourFilterSorterSearcher filterSorterSearcher = null)
        {
            GetToursQueryFactory getToursQueryFactory = App.DI.Resolve<GetToursQueryFactory>();

            IQuery getToursQuery = filterSorterSearcher == null || !filterSorterSearcher.IsChanged ? getToursQueryFactory.GetAllTours() : getToursQueryFactory.GetTours(filterSorterSearcher);
            IServerResponse tours = await App.DI.Resolve<IServerCommunicator>().SendQuery(getToursQuery);
            return App.DI.Resolve<IResponseParser>().ParseCollection<Tour, ResponseException>(tours).ToList();
        }

        public async Task<Tour> GetTourById(long id)
        {
            IQuery getTourQuery = App.DI.Resolve<GetToursQueryFactory>().GetTourById(id);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(getTourQuery);
            return App.DI.Resolve<IResponseParser>().Parse<Tour, ResponseException>(response);
        }

        public async Task<bool> CheckJoinAbility(User user, Tour tour)
        {
            IQuery checkJoinAbilityQuery = await App.DI.Resolve<CheckTourJoinAbilityQueryFactory>().CheckTourJoinAbility(user, tour);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(checkJoinAbilityQuery);
            return App.DI.Resolve<IResponseParser>().Parse<BoolResult, ResponseException>(response).result;
        }

        public async Task<bool> JoinPrepare(Tour tour, LiqpayPayment payment)
        {
            IQuery prepareJoinQuery = await App.DI.Resolve<JoinPrepareQueryFactory>().PrepareJoin(App.DI.Resolve<Session>().CurrentUser, tour, payment);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(prepareJoinQuery, App.DI.Resolve<CookieContainer>());
            return App.DI.Resolve<IResponseParser>().Parse<BoolResult, ResponseException>(response).result;
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

        public async Task<IEnumerable<User>> GetTourMembers(Tour tour)
        {
            IQuery getTourMembersQuery = await App.DI.Resolve<GetTourMembersQueryFactory>().GetTourMembers(tour);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(getTourMembersQuery);
            return App.DI.Resolve<IResponseParser>().ParseCollection<User, ResponseException>(response);
        }

        public async Task<Participating> FinishTour(Tour tour, int tourRate, int guideRate)
        {
            User user = App.DI.Resolve<Session>().CurrentUser;
            Participating participating = new Participating(tourRate, guideRate);

            IQuery finishTourQuery = await App.DI.Resolve<FinishTourQueryFactory>().FinishTour(user, tour, participating);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(finishTourQuery);
            return App.DI.Resolve<IResponseParser>().Parse<Participating, ResponseException>(response);
        }

        public async Task<ParticipatingStatus> GetParticipatingStatus(User user, Tour tour)
        {
            IQuery getParticipatingStatusQuery = await App.DI.Resolve<GetParticipatingStatusQueryFactory>().GetParticipatingStatus(user, tour);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(getParticipatingStatusQuery);
            return App.DI.Resolve<IResponseParser>().Parse<ParticipatingStatus, ResponseException>(response);
        }

        public async Task<DoubleResult> GetAvgTourRate(Tour tour)
        {
            IQuery getTourAvgRatingQuery = await App.DI.Resolve<GetAvgTourRatingQueryFactory>().GetAvgTourRating(tour);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(getTourAvgRatingQuery);
            return App.DI.Resolve<IResponseParser>().Parse<DoubleResult, ResponseException>(response);
        }

        public async Task<Tour> CreateCustomTour(string name, string description, string location, DateTime start, DateTime finish)
        {
            CustomTour tour = new CustomTour(name, description, location, start, finish);
            User user = App.DI.Resolve<Session>().CurrentUser;

            IQuery createCustomTourQuery = await App.DI.Resolve<CreateCustomTourQueryFactory>().CreateCustomTour(tour, user);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(createCustomTourQuery);
            return App.DI.Resolve<IResponseParser>().Parse<Tour, ResponseException>(response);
        }
    }
}
