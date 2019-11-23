using System.Threading.Tasks;

using Autofac;

using GoNTrip.Model;
using GoNTrip.Pages;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Controllers
{
    public class UpdateProfileController
    {
        public async Task<User> Update(User user)
        {
            IQuery updateProfileQuery = await App.DI.Resolve<UpdateProfileQueryFactory>().UpdateProfile(user);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(updateProfileQuery);
            return App.DI.Resolve<IResponseParser>().Parse<User, ResponseException>(response);
        }
    }
}
