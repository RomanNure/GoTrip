using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

using Newtonsoft.Json;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class AuthQueryFactory : QueryFactory
    {
        /*protected const string LOGIN_KEY = "login";
        protected const string PASSWORD_KEY = "password";
        protected const string EMAIL_KEY = "email";*/

        private const string SIGN_UP_SERVER_METHOD_NAME = "register";
        public IQuery SignUp(string login, string password, string email) =>
            new Query(QueryMethod.POST, SIGN_UP_SERVER_METHOD_NAME, Query.APP_JSON, ExtractJsonQueryBody<User, SignUpField>(new User(login, password, email)));

        private const string LOG_IN_SERVER_METHOD_NAME = "authorize";
        public IQuery LogIn(string login, string password) =>
            new Query(QueryMethod.POST, LOG_IN_SERVER_METHOD_NAME, Query.APP_JSON, ExtractJsonQueryBody<User, LogInField>(new User(login, password)));
    }
}
