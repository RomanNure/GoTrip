using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class LogInQueryFactory : QueryFactory
    {
        private const string LOG_IN_SERVER_METHOD_NAME = "authorize";
        public async Task<IQuery> LogIn(string login, string password) => 
            new Query(QueryMethod.POST, LOG_IN_SERVER_METHOD_NAME, await ExtractJsonQueryBody<User, LogInField>(new User(login, password)));
    }
}
