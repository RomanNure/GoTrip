using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class AddGuideQueryFactory : QueryFactory
    {
        private const string ADD_GUIDE_SERVER_METHOD_NAME = "guide/add";

        public async Task<IQuery> AddGuide(User user, Guide guide) =>
            new Query(QueryMethod.POST, ADD_GUIDE_SERVER_METHOD_NAME, await ExtractJsonQueryBody<User, Guide, AddGuideField>(user, guide));
    }
}
