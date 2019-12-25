using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class FinishTourQueryFactory : QueryFactory
    {
        private const string FINISH_TOUR_SERVER_METHOD_NAME = "rate/tour";
        public async Task<IQuery> FinishTour(User user, Tour tour, Participating participating) =>
            new Query(QueryMethod.POST, FINISH_TOUR_SERVER_METHOD_NAME, 
                await ExtractJsonQueryBody<User, Tour, Participating, FinishTour>(user, tour, participating));
    }
}
