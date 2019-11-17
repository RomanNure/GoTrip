using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
        public async Task<Session> LogIn(string login, string password)
        {
            IQuery logInQuery = await App.DI.Resolve<LogInQueryFactory>().LogIn(login, password);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(logInQuery, App.DI.Resolve<CookieContainer>());

            Session session = App.DI.Resolve<Session>();
            session.CurrentUser = App.DI.Resolve<IResponseParser>().Parse<User>(response);
            return session;
        }
    }
}
