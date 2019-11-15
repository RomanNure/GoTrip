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
        public async Task<User> SignUp(string login, string password, string email)
        {
            IQuery signUpQuery = await App.DI.Resolve<SignUpQueryFactory>().SignUp(login, password, email);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(signUpQuery, App.DI.Resolve<CookieContainer>());
            return App.DI.Resolve<IResponseParser>().Parse<User>(response);
        }
    }
}
