using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using GoNTrip.Model;
using GoNTrip.Pages;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Controllers
{
    public class GuideController
    {
        private const string KEYWORDS_GLUE = ",";

        public async Task<Guide> Add(IEnumerable<string> keywrods, string card)
        {
            Guide newGuide = new Guide(string.Join(KEYWORDS_GLUE, keywrods), card);
            User currentUser = App.DI.Resolve<Session>().CurrentUser;

            IQuery addGuideQuery = await App.DI.Resolve<AddGuideQueryFactory>().AddGuide(currentUser, newGuide);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(addGuideQuery);
            return App.DI.Resolve<IResponseParser>().Parse<Guide, ResponseException>(response);
        }

        public async Task<bool> CheckGuidingAbility(Tour tour)
        {
            Guide guide = App.DI.Resolve<Session>().CurrentUser.guide;

            if (guide == null)
                return false;

            IQuery checkGuidingAbilityQuery = await App.DI.Resolve<CheckGuidingAbilityQueryFactory>().CheckGuidingAbility(tour, guide);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(checkGuidingAbilityQuery);
            return App.DI.Resolve<IResponseParser>().Parse<BoolResult, ResponseException>(response).result;
        }

        public async Task<RawNotification> OfferGuiding(Tour tour, double sum)
        {
            User user = App.DI.Resolve<Session>().CurrentUser;

            if (user.guide == null)
                throw new ResponseException("You're not a guide");

            IQuery offerGuidingQuery = await App.DI.Resolve<OfferGuidingQueryFactory>().OfferGuiding(tour, user.guide, new Money(sum));
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(offerGuidingQuery);
            return App.DI.Resolve<IResponseParser>().Parse<RawNotification, ResponseException>(response);
        }
    }
}
