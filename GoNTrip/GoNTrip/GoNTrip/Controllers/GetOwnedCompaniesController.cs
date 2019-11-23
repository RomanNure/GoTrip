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
    public class GetOwnedCompaniesController
    {
        public async Task<List<Company>> GetOwnedCompanies(User user)
        {
            IQuery getOwnedCompaniesQuery = await App.DI.Resolve<GetOwnedCompaniesQueryFactory>().GetOwnedCompanies(user);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(getOwnedCompaniesQuery);
            return App.DI.Resolve<IResponseParser>().ParseCollection<Company, ResponseException>(response).ToList();
        }
    }
}
