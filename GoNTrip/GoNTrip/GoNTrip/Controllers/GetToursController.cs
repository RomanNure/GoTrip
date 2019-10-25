using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using System.Linq;
using GoNTrip.Pages;
using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Controllers
{
    public class GetToursController
    {
        public async Task<List<Tour>> GetAllTours()
        {
            IQuery getAllToursQuery = App.DI.Resolve<GetToursQueryFactory>().GetAllTours();
            IServerResponse allTours = await App.DI.Resolve<IServerCommunicator>().SendQuery(getAllToursQuery);
            return App.DI.Resolve<IResponseParser>().ParseCollection<Tour>(allTours).ToList();
        }
    }
}
