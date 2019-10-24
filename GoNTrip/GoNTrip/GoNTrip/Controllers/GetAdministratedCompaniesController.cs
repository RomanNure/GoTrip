using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using GoNTrip.Pages;
using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Controllers
{
    public class GetAdministratedCompaniesController
    {
        public async Task<List<Company>> GetAdministratedCompanies(User user)
        {
            IQuery getAdministratedCompaniesQuery = await App.DI.Resolve<GetAdministratedCompaniesQueryFactory>().GetAdministratedCompanies(user);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(getAdministratedCompaniesQuery);
            return App.DI.Resolve<IResponseParser>().ParseCollection<Company>(response).ToList();
        }
    }
}
