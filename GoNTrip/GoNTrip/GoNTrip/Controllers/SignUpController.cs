using Autofac;

using GoNTrip.Model;
using GoNTrip.Pages;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Controllers
{
    public class SignUpController
    {
        public User SignUp(string login, string password, string email)
        {
            IQuery signUpQuery = App.DI.Resolve<SignUpQueryFactory>().SignUp(login, password, email);
            IServerResponse response = App.DI.Resolve<IServerCommunicator>().SendQuery(signUpQuery);
            return App.DI.Resolve<IResponseParser>().Parse<User>(response);
        }
    }
}
