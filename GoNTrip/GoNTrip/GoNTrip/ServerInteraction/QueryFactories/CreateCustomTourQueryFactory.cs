using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class CreateCustomTourQueryFactory : QueryFactory
    {
        private const string CREATE_CUSTOM_TOUR_SERVER_METHOD_NAME = "custom/tour/create";
        public async Task<IQuery> CreateCustomTour(CustomTour tour, User creator) =>
            new Query(QueryMethod.POST, CREATE_CUSTOM_TOUR_SERVER_METHOD_NAME,
                await ExtractJsonQueryBody<CustomTour, User, CustomTourCreate>(tour, creator));
    }
}
