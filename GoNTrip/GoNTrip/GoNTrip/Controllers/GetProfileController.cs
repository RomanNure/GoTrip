using System.Threading.Tasks;

using Autofac;

using GoNTrip.Model;
using GoNTrip.Pages;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Controllers
{
    public class GetProfileController
    {
        public async Task<User> GetUserById(long id)
        {
            IQuery getUserQuery = await App.DI.Resolve<GetProfileQueryFactory>().GetUserById(id);
            IServerResponse response = await App.DI.ResolveOptional<IServerCommunicator>().SendQuery(getUserQuery);
            return App.DI.Resolve<IResponseParser>().Parse<User>(response);
        }
    }
}
