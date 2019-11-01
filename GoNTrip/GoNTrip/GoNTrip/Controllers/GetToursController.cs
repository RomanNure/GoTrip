using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using System.Linq;
using GoNTrip.Pages;
using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.Model.FilterSortSearch;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Controllers
{
    public class GetToursController
    {
        public async Task<List<Tour>> GetTours(FilterSorterSearcher filterSorterSearcher = null)
        {
            GetToursQueryFactory getToursQueryFactory = App.DI.Resolve<GetToursQueryFactory>();

            IQuery getToursQuery = filterSorterSearcher == null || filterSorterSearcher.IsEmpty() ? getToursQueryFactory.GetAllTours() : getToursQueryFactory.GetTours(filterSorterSearcher);
            IServerResponse tours = await App.DI.Resolve<IServerCommunicator>().SendQuery(getToursQuery);
            return App.DI.Resolve<IResponseParser>().ParseCollection<Tour>(tours).ToList();
        }
    }
}
