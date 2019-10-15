using System.IO;
using System.Threading.Tasks;

using Autofac;

using GoNTrip.Pages;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.QueryFactories;

namespace GoNTrip.Controllers
{
    public class ChangeAvatarController
    {
        public async Task<string> ChangeAvatar(long id, Stream avatar)
        {
            IQuery changeAvatarQuery = await App.DI.Resolve<ChangeAvatarQueryFactory>().ChangeAvatar(id, avatar);
            IServerResponse response = await App.DI.ResolveOptional<IServerCommunicator>().SendQuery(changeAvatarQuery);
            return response.Data;
        }
    }
}
