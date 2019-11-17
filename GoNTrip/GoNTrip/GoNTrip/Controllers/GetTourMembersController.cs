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
    public class GetTourMembersController
    {
        public async Task<IEnumerable<User>> GetTourMembers(Tour tour)
        {
            IQuery getTourMembersQuery = await App.DI.Resolve<GetTourMembersQueryFactory>().GetTourMembers(tour);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(getTourMembersQuery);
            return App.DI.Resolve<IResponseParser>().ParseCollection<User>(response);
        }
    }
}
