using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class GetProfileQueryFactory : QueryFactory
    {
        private const string GET_USER_SERVER_METHOD_NAME = "user/get";
        public async Task<IQuery> GetUserById(long id) =>
            new Query(QueryMethod.GET, GET_USER_SERVER_METHOD_NAME, parameters: await ExtractQueryParameters<User, GetProfileField>(new User(id)));
    }
}
