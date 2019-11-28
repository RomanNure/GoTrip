using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class OfferGuidingQueryFactory : QueryFactory
    {
        private const string OFFER_GUIDING_SERVER_METHOD_NAME = "guide/invitation/fromguide";

        public async Task<IQuery> OfferGuiding(Tour tour, Guide guide, Money money) =>
            new Query(QueryMethod.POST, OFFER_GUIDING_SERVER_METHOD_NAME,
                      await ExtractJsonQueryBody<Tour, Guide, Money, OfferGuidingField>(tour, guide, money));
    }
}
