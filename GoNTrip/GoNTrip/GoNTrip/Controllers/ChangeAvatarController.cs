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
        public async Task<User> ChangeAvatar(User user, Stream avatar)
        {
            IQuery changeAvatarQuery = await App.DI.Resolve<ChangeAvatarQueryFactory>().ChangeAvatar(user.id, avatar);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(changeAvatarQuery);
            user.UpdateAvatarUrl(ServerCommunicator.MULTIPART_SERVER_URL + "/" + App.DI.Resolve<IResponseParser>().Parse<FilePath, ResponseException>(response).path);
            return user;
        }
    }
}
