using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoNTrip.Model.FilterSortSearch.Tour
{
    [JsonObject]
    public class TourFilterSorterSearcher
    {
        public TourFilters filters { get; private set; }
        public TourSearcher search { get; private set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TourSortCriteria sortingCriterion { get; set; }

        public TourFilterSorterSearcher()
        {
            this.filters = new TourFilters();
            this.search = new TourSearcher();
            this.sortingCriterion = TourSortCriteria.no;
        }

        public void FillFilters(double minPrice, double maxPrice, DateTime minStart, DateTime maxStart, int minPlaces, int maxPlaces)
        {
            this.filters.priceFilter.from = minPrice;
            this.filters.priceFilter.to = maxPrice;

            this.filters.startDateFilter.from = minStart;
            this.filters.startDateFilter.to = maxStart;

            this.filters.participantsFilter.from = minPlaces;
            this.filters.participantsFilter.to = maxPlaces;
        }

        public void FillSearch(string tourNameSubstr, string tourLocationSubstr)
        {
            this.search.tourNameSubstr = tourNameSubstr;
            this.search.tourLocationSubstr = tourLocationSubstr;
        }

        [JsonIgnore]
        public bool IsChanged => filters.IsChanged || sortingCriterion != TourSortCriteria.no || !search.IsEmpty();
    }
}
