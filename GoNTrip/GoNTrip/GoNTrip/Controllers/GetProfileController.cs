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
        public User GetUserById(long id)
        {
            IQuery getUserQuery = App.DI.Resolve<GetProfileQueryFactory>().GetUserById(id);
            IServerResponse response = App.DI.ResolveOptional<IServerCommunicator>().SendQuery(getUserQuery);
            return App.DI.Resolve<IResponseParser>().Parse<User>(response);
        }
    }
}
