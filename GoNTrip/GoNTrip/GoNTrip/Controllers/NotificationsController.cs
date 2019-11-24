using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using GoNTrip.Model;
using GoNTrip.Pages;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Controllers
{
    public class NotificationsController
    {
        public async Task<List<Notification>> GetNotifications()
        {
            IQuery getNotificationsQuery = await App.DI.Resolve<GetUserNotificationsQueryFactory>().GetUserNotifications(App.DI.Resolve<Session>().CurrentUser);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(getNotificationsQuery);
            return App.DI.Resolve<IResponseParser>().ParseCollection<Notification, ResponseException>(response).ToList();
        }
    }
}
