using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class GetProfileQueryFactory : QueryFactory
    {
        private const string GET_USER_SERVER_METHOD_NAME = "user/get";
        public IQuery GetUserById(long id) =>
            new Query(QueryMethod.GET, GET_USER_SERVER_METHOD_NAME, parameters: ExtractQueryParameters<User, GetProfileFiled>(new User(id)));
    }
}
