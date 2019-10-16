using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class UpdateProfileQueryFactory : QueryFactory
    {
        private const string UPDATE_PROFILE_SERVER_METHOD_NAME = "update/user";
        public async Task<IQuery> UpdateProfile(User user) =>
            new Query(QueryMethod.POST, UPDATE_PROFILE_SERVER_METHOD_NAME, await ExtractJsonQueryBody<User, UpdateProfileField>(user));
    }
}
