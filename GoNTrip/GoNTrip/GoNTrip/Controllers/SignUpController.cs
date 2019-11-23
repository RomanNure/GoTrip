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
    public class SignUpController
    {
        public async Task<Session> SignUp(string login, string password, string email)
        {
            IQuery signUpQuery = await App.DI.Resolve<SignUpQueryFactory>().SignUp(login, password, email);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(signUpQuery, App.DI.Resolve<CookieContainer>());

            Session session = App.DI.Resolve<Session>();
            session.CurrentUser = App.DI.Resolve<IResponseParser>().Parse<User, ResponseException>(response);
            return session;
        }
    }
}
