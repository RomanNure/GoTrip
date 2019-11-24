using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<Session> SignUp(string login, string password, string email)
        {
            IServerCommunicator server = App.DI.Resolve<IServerCommunicator>();
            IQuery signUpQuery = await App.DI.Resolve<SignUpQueryFactory>().SignUp(login, password, email);
            IServerResponse response = await server.SendQuery(signUpQuery, App.DI.Resolve<CookieContainer>());

            Session session = App.DI.Resolve<Session>();
            session.CurrentUser = App.DI.Resolve<IResponseParser>().Parse<User, ResponseException>(response);
            session.SessionId = response.CookieContainer.GetCookies(new Uri(server.ServerURL))[Constants.SESSION_ID_HEADER_NAME]?.Value;
            return session;
        }
    }
}
