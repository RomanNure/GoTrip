using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class OfferGuidingCustomTourQueryFactory : QueryFactory
    {
        private const string OFFER_GUIDING_CUSTOM_TOUR_SERVER_METHOD_NAME = "custom/guide/invitation/fromguide";
        public async Task<IQuery> OfferGuiding(Tour tour, Guide guide, Money money) =>
            new Query(QueryMethod.POST, OFFER_GUIDING_CUSTOM_TOUR_SERVER_METHOD_NAME,
                      await ExtractJsonQueryBody<Tour, Guide, Money, OfferGuidingField>(tour, guide, money));
    }
}
