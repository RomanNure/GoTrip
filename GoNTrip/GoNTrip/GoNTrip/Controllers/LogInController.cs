using Autofac;

using GoNTrip.Model;
using GoNTrip.Pages;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Controllers
{
    public class LogInController
    {
        public User LogIn(string login, string password)
        {
            IQuery logInQuery = App.DI.Resolve<LogInQueryFactory>().LogIn(login, password);
            IServerResponse response = App.DI.Resolve<IServerCommunicator>().SendQuery(logInQuery);
            return App.DI.Resolve<IResponseParser>().Parse<User>(response);
        }
    }
}
