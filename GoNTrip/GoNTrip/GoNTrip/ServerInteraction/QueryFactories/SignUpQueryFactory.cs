using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class SignUpQueryFactory : QueryFactory
    {
        private const string SIGN_UP_SERVER_METHOD_NAME = "register";
        public async Task<IQuery> SignUp(string login, string password, string email) => 
            new Query(QueryMethod.POST, SIGN_UP_SERVER_METHOD_NAME, await ExtractJsonQueryBody<User, SignUpField>(new User(login, password, email)));
    }
}
