using System.IO;
using System.Threading.Tasks;

using Autofac;

using GoNTrip.Model;
using GoNTrip.Pages;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Controllers
{
    public class ChangeAvatarController
    {
        public async Task<FilePath> ChangeAvatar(long id, Stream avatar)
        {
            IQuery changeAvatarQuery = await App.DI.Resolve<ChangeAvatarQueryFactory>().ChangeAvatar(id, avatar);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(changeAvatarQuery);
            return App.DI.Resolve<IResponseParser>().Parse<FilePath>(response);
        }
    }
}
