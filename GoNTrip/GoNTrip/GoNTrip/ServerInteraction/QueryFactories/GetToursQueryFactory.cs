using System.Collections.Generic;

using Newtonsoft.Json;

using GoNTrip.ServerAccess;
using GoNTrip.Model.FilterSortSearch.Tour;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class GetToursQueryFactory : QueryFactory
    {
        public const string GET_ALL_TOURS_SERVER_METHOD_NAME = "tours/get";
        public IQuery GetAllTours() => new Query(QueryMethod.GET, GET_ALL_TOURS_SERVER_METHOD_NAME);

        public const string GET_TOURS_SERVER_METHOD_NAME = "tours/get/advanced";
        public IQuery GetTours(TourFilterSorterSearcher filterSorterSearcher) => 
            new Query(QueryMethod.POST, GET_TOURS_SERVER_METHOD_NAME, JsonConvert.SerializeObject(filterSorterSearcher));

        public const string GET_TOUR_BY_ID_SERVER_METHOD_NAME = "tours/get/id";
        public const string TOUR_ID_PARAM_NAME = "id";

        public IQuery GetTourById(long id) =>
            new Query(QueryMethod.GET, GET_TOUR_BY_ID_SERVER_METHOD_NAME, 
                      parameters: new Dictionary<string, string>() { { TOUR_ID_PARAM_NAME, id.ToString() } });
    }
}
