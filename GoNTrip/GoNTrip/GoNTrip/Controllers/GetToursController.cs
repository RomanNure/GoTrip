using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using GoNTrip.Pages;
using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.Model.FilterSortSearch.Tour;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Controllers
{
    public class GetToursController
    {
        public async Task<List<Tour>> GetTours(TourFilterSorterSearcher filterSorterSearcher = null)
        {
            GetToursQueryFactory getToursQueryFactory = App.DI.Resolve<GetToursQueryFactory>();

            IQuery getToursQuery = filterSorterSearcher == null || !filterSorterSearcher.IsChanged ? getToursQueryFactory.GetAllTours() : getToursQueryFactory.GetTours(filterSorterSearcher);
            IServerResponse tours = await App.DI.Resolve<IServerCommunicator>().SendQuery(getToursQuery);
            return App.DI.Resolve<IResponseParser>().ParseCollection<Tour, ResponseException>(tours).ToList();
        }
    }
}
