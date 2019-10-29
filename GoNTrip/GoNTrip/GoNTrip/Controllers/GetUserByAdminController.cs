using System.Threading.Tasks;

using Autofac;

using GoNTrip.Model;
using GoNTrip.Pages;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Controllers
{
    public class GetUserByAdminController
    {
        public async Task<User> GetUserByAdmin(Admin admin)
        {
            IQuery getUserByAdminQuery = await App.DI.Resolve<GetUserByAdminQueryFactory>().GetUserByAdmin(admin);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(getUserByAdminQuery);
            return App.DI.Resolve<IResponseParser>().Parse<User>(response);
        }
    }
}
