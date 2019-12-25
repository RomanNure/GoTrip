using System;
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
            IServerCommunicator server = App.DI.Resolve<IServerCommunicator>();
            IQuery logInQuery = await App.DI.Resolve<LogInQueryFactory>().LogIn(login, password);
            IServerResponse response = await server.SendQuery(logInQuery, App.DI.Resolve<CookieContainer>());

            Session session = App.DI.Resolve<Session>();
            session.CurrentUser = App.DI.Resolve<IResponseParser>().Parse<User, ResponseException>(response);
            session.SessionId = response.CookieContainer.GetCookies(new Uri(server.ServerURL))[Constants.SESSION_ID_HEADER_NAME]?.Value;
            return session;
        }
    }
}
