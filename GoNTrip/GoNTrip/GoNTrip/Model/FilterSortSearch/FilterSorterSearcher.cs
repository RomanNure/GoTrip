using System.Collections.Generic;

using Newtonsoft.Json;

namespace GoNTrip.Model.FilterSortSearch
{
    [JsonObject]
    public class FilterSorterSearcher
    {
        public List<Filter> filters { get; private set; }
        public string[] sortingCriteria { get; private set; }
        public string tourSubstring { get; set; }
        public string locationSubstring { get; set; }

        [JsonIgnore]
        private Sorter criteria = Sorter.no;

        [JsonIgnore]
        public Sorter SortingCriteria { get { return criteria; } set { criteria = value; sortingCriteria[0] = criteria.ToString(); } }

        public FilterSorterSearcher(List<Filter> filters = null, Sorter sortingCriteria = Sorter.no, string tourSubstring = "", string locationSubstring = "")
        {
            this.filters = filters == null ? new List<Filter>() : filters;
            this.sortingCriteria = new string[1];
            this.SortingCriteria = sortingCriteria;
            this.tourSubstring = tourSubstring;
            this.locationSubstring = locationSubstring;
        }

        public bool IsEmpty() => filters.Count == 0 && criteria == Sorter.no && tourSubstring == "" && locationSubstring == "";
    }
}
