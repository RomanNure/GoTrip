using GoNTrip.ServerAccess;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class AuthQueryFactory
    {
        protected const string LOGIN_KEY = "login";
        protected const string PASSWORD_KEY = "password";
        protected const string EMAIL_KEY = "email";

        private const string SIGN_UP_SERVER_METHOD_NAME = "register";
        public IQuery SignUp(string login, string password, string email)
        {
            IQuery signUpQuery = new Query(QueryMethod.POST, SIGN_UP_SERVER_METHOD_NAME);

            signUpQuery.Parameters.Add(LOGIN_KEY, login);
            signUpQuery.Parameters.Add(PASSWORD_KEY, password);
            signUpQuery.Parameters.Add(EMAIL_KEY, email);

            return signUpQuery;
        }

        private const string LOG_IN_SERVER_METHOD_NAME = "authorize";
        public IQuery LogIn(string login, string password)
        {
            IQuery logInQuery = new Query(QueryMethod.POST, LOG_IN_SERVER_METHOD_NAME);

            logInQuery.Parameters.Add(LOGIN_KEY, login);
            logInQuery.Parameters.Add(PASSWORD_KEY, password);

            return logInQuery;
        }
    }
}
